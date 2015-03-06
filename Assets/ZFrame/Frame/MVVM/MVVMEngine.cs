using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using ZFrame.Base.MonoBase;
using ZFrame.Utilities;

namespace ZFrame.Frame.MVVM
{
    public class MVVMEngine : MonoSingleton<MVVMEngine>
    {
        private readonly List<BindingGroup> _groups = new List<BindingGroup>();
        private readonly Dictionary<Type, object> _viewModels = new Dictionary<Type, object>();
        private readonly List<object> _views = new List<object>();

        public void Register<T>(T bindable) where T : class
        {
            if (Attribute.IsDefined(bindable.GetType(), typeof (ViewModelAttribute)))
            {
                if (!_viewModels.TryAdd(bindable.GetType(), bindable))
                {
                    Debug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}",
                        bindable.GetType(), bindable));
                }
            }
            if (Attribute.IsDefined(bindable.GetType(), typeof (ViewAttribute)))
            {
                if (!_views.SafeAdd(bindable))
                {
                    Debug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}",
                        bindable.GetType(), bindable));
                }
            }

            _groups.Clear();

            foreach (object view in _views)
            {
                Type viewModelType =
                    ((ViewAttribute) Attribute.GetCustomAttribute(view.GetType(), typeof (ViewAttribute)))
                        .SourceType;
                if (viewModelType == null)
                {
                    Debug.LogError(string.Format("View class has no viewModel class! view type: {0}", view.GetType()));
                    continue;
                }
                object viewModel = _viewModels.TryGet(viewModelType);
                if (viewModel == null)
                {
                    Debug.LogWarning(
                        string.Format(
                            "It may happend when view has been registered but view model has not yet. View: {0}, ViewModel: {1}",
                            view,
                            viewModelType));
                    continue;
                }

                foreach (
                    MemberInfo member in
                        view.GetType().GetMembers().Where(p => Attribute.IsDefined(p, typeof (BindingMemberAttribute))))
                {
                    BindingGroup group = new BindingGroup();
                    group.ViewMember = member;
                    group.View = view;
                    group.ViewModel = viewModel;

                    string bindingKey =
                        ((BindingMemberAttribute) Attribute.GetCustomAttribute(member, typeof (BindingMemberAttribute)))
                            .BindingKey;
                    switch (member.MemberType)
                    {
                        case MemberTypes.Event:
                        {
                            Delegate del = Delegate.CreateDelegate(((EventInfo) member).EventHandlerType, viewModel,
                                bindingKey, false,
                                false);
                            if (del == null)
                                Debug.LogError(
                                    string.Format("Binding method to event failed! event: {0}, methodName: {1}", member,
                                        bindingKey));
                            else
                                ((EventInfo) member).AddEventHandler(view, del);
                        }
                            break;
                        case MemberTypes.Field:
                            break;
                        case MemberTypes.Method:
                        {
                            MethodInfo method = viewModelType.GetMethod(bindingKey);

                            if (method.ReturnType == ((MethodInfo) member).ReturnType)
                            {
                                if (method.GetParameters().Length == ((MethodInfo) member).GetParameters().Length)
                                {
                                    bool ok = true;
                                    for (int i = 0; i < method.GetParameters().Length; i++)
                                        if (method.GetParameters()[i].ParameterType !=
                                            ((MethodInfo) member).GetParameters()[i].ParameterType)
                                            ok = false;
                                    if (ok)
                                    {
                                        group.ViewModelMember = method;
                                        break;
                                    }
                                }
                            }

                            Debug.LogError(string.Format("View method does not match view model method! {0}, {1}",
                                member, method));
                        }
                            break;
                        case MemberTypes.Property:
                        {
                            if (!((PropertyInfo) member).CanRead || !((PropertyInfo) member).CanWrite)
                            {
                                Debug.LogError(
                                    string.Format("View property is not accessable! {2} Read: {0}, Write: {1}",
                                        ((PropertyInfo) member).CanRead, ((PropertyInfo) member).CanWrite, member));
                                break;
                            }
                            PropertyInfo prop = viewModelType.GetProperty(bindingKey);
                            if (!prop.CanRead || !prop.CanWrite)
                            {
                                Debug.LogError(
                                    string.Format("View model property is not accessable! {2} Read: {0}, Write: {1}",
                                        prop.CanRead, prop.CanWrite, prop));
                                break;
                            }
                            if (((PropertyInfo) member).PropertyType != prop.PropertyType)
                            {
                                Debug.LogError(
                                    string.Format("View property does not match view model property! {0}, {1}",
                                        member, prop));
                                break;
                            }
                            group.ViewModelMember = prop;
                        }
                            break;
                        default:
                            Debug.LogError("Member type not surported for binding! type: " + member.MemberType);
                            break;
                    }

                    if (group.CanUpdate)
                    {
                        _groups.Add(group);
                    }
                }
            }

            foreach (object viewModel in _viewModels.Values)
            {
                if (viewModel is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged) viewModel).PropertyChanged +=
                        (sender, args) => Notify(sender, args.PropertyName);
                }
            }
        }

        #region Property OnNotify

        private void Notify(PropertyInfo prop)
        {
            foreach (BindingGroup group in _groups.Where(g => g.CanUpdate))
            {
                if (group.ViewModelMember == prop)
                {
                    ((PropertyInfo) group.ViewMember).SetValue(group.View, prop.GetValue(group.ViewModel, null), null);
                }
                else if (group.ViewMember == prop)
                {
                    ((PropertyInfo) group.ViewModelMember).SetValue(group.ViewModel, prop.GetValue(group.View, null),
                        null);
                }
            }
        }

        public void Notify<T>(T sender, string propName) where T : class
        {
            if (sender == null)
            {
                Debug.LogError(string.Format("Notify property sender is null! type: {0}, propertyName: {1}", typeof (T),
                    propName));
                return;
            }
            if (string.IsNullOrEmpty(propName))
            {
                Debug.LogError(
                    string.Format("Notify property Name cannot be null or empty! sender: {0}, propertyName: {1}", sender,
                        propName));
                return;
            }

            PropertyInfo prop = sender.GetType().GetProperty(propName);
            if (prop == null)
            {
                Debug.LogError(string.Format("Notify property not found! sender: {0}, propertyName: {1}", sender,
                    propName));
                return;
            }
            Notify(prop);
        }

        public void Notify<T>(Expression<Func<T>> exp)
        {
            MemberExpression bodyExpr = exp.Body as MemberExpression;
            if (bodyExpr == null)
            {
                throw new ArgumentException("Expression must be a MemberExpression!", "exp");
            }
            PropertyInfo propInfo = bodyExpr.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException("Expression must be a PropertyExpression!", "exp");
            }

            Notify(propInfo);
        }

        #endregion

        #region Method notify

        public void Notify(Action method)
        {
            Invoke(method.Method, null);
        }

        public void Notify<T>(Action<T> method, T parameter)
        {
            Invoke(method.Method, new object[] {parameter});
        }

        public void Notify<T1, T2>(Action<T1, T2> method, T1 param1, T2 param2)
        {
            Invoke(method.Method, new object[] {param1, param2});
        }

        public void Notify<T1, T2, T3>(Action<T1, T2, T3> method, T1 param1, T2 param2, T3 param3)
        {
            Invoke(method.Method, new object[] {param1, param2, param3});
        }

        public void Notify<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            Invoke(method.Method, new object[] {param1, param2, param3, param4});
        }

        private void Invoke(MethodInfo method, object[] parameters)
        {
            foreach (BindingGroup group in _groups.Where(g => g.CanUpdate))
            {
                if (group.ViewModelMember == method)
                {
                    ((MethodInfo) group.ViewMember).Invoke(group.View, parameters);
                }
                else if (group.ViewMember == method)
                {
                    ((MethodInfo) group.ViewModelMember).Invoke(group.ViewModel, parameters);
                }
            }
        }

        #endregion

        private class BindingGroup
        {
            public object ViewModel { get; set; }
            public MemberInfo ViewModelMember { get; set; }
            public object View { get; set; }
            public MemberInfo ViewMember { get; set; }

            public bool CanUpdate
            {
                get { return ViewModel != null && ViewModelMember != null && View != null && ViewMember != null; }
            }
        }
    }
}