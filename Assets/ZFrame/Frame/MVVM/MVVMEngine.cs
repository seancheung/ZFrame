using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZFrame.Debugger;

namespace ZFrame.Frame.MVVM
{
	public class MVVMEngine : MonoSingleton<MVVMEngine>, IZDisposable
	{
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

		private readonly List<BindingGroup> _groups = new List<BindingGroup>();
		private readonly Dictionary<Type, object> _viewModels = new Dictionary<Type, object>();
		private readonly List<object> _views = new List<object>();

		public void Register<T>(T bindable) where T : class
		{
			if (Attribute.IsDefined(bindable.GetType(), typeof (ViewModelAttribute)))
			{
				if (!_viewModels.TryAdd(bindable.GetType(), bindable))
				{
					ZDebug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}", bindable.GetType(), bindable));
				}
			}
			if (Attribute.IsDefined(bindable.GetType(), typeof (ViewAttribute)))
			{
				if (!_views.SafeAdd(bindable))
				{
					ZDebug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}", bindable.GetType(), bindable));
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
					ZDebug.LogError(string.Format("Binding source class has no target class! source type: {0}", view.GetType()));
					continue;
				}
				object viewModel = _viewModels.TryGet(viewModelType);

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
						case MemberTypes.Constructor:
							break;
						case MemberTypes.Event:
							((EventInfo) member).AddEventHandler(view,
								Delegate.CreateDelegate(((EventInfo) member).EventHandlerType, viewModel, bindingKey));
							break;
						case MemberTypes.Field:
							break;
						case MemberTypes.Method:
							group.ViewModelMember = viewModelType.GetMethod(bindingKey);
							break;
						case MemberTypes.Property:
							group.ViewModelMember = viewModelType.GetProperty(bindingKey);
							break;
						case MemberTypes.TypeInfo:
							break;
						case MemberTypes.Custom:
							break;
						case MemberTypes.NestedType:
							break;
						case MemberTypes.All:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					if (group.CanUpdate)
					{
						_groups.Add(group);
					}
				}
			}

			foreach (var viewModel in _viewModels.Values)
			{
				if (viewModel is INotifyPropertyChanged)
				{
					((INotifyPropertyChanged)viewModel).PropertyChanged += (sender, args) => Notify(sender, args.PropertyName);
				}
			}
		}

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
					((PropertyInfo) group.ViewModelMember).SetValue(group.ViewModel, prop.GetValue(group.View, null), null);
				}
			}
		}

		public void Notify<T>(T view, string propName) where T : class
		{
			if (view == null)
			{
				ZDebug.LogError(string.Format("Notify property changed view class is null! type: {0}, property: {1}", typeof(T),
					propName));
				return;
			}
			Notify(view.GetType().GetProperty(propName));
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

		public bool DisposeOnApplicationQuit()
		{
			return false;
		}

		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}
	}
}