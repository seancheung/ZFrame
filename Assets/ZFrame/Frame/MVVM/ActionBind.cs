using System;
using System.Collections.Generic;
using System.Reflection;
using Fasterflect;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

public class ActionBind : MonoBehaviour
{
    [SerializeField] private List<MonoAction> bindingGroups = new List<MonoAction>();

    private void Awake()
    {
        bindingGroups.ForEach(g => g.SetMembers());
    }

    [Serializable]
    public class MonoAction
    {
        [SerializeField] private Component component;
        [SerializeField] private string action;
        [SerializeField] private MonoMethod method;

        private Object _safeObject;

        public void SetMembers()
        {
            if (component == null || string.IsNullOrEmpty(action))
                return;

            string[] actionParams = action.Split(new[] {'/', '(', ')'});
            if (actionParams[0] == typeof (GameObject).Name())
                _safeObject = component.gameObject;
            else
                _safeObject = component;

            EventInfo act = _safeObject.GetType().GetEvent(actionParams[1]);
            if (act == null)
                return;

            method.TryAddDelegate(act, _safeObject);
        }
    }

    [Serializable]
    public class MonoMethod
    {
        public Component component;
        public string method;
        private Object _safeObject;

        public void TryAddDelegate(EventInfo act, Object target)
        {
            if (component == null || string.IsNullOrEmpty(method))
                return;
            string[] methodParams = method.Split('/');
            if (methodParams[0] == typeof (GameObject).Name())
                _safeObject = component.gameObject;
            else
                _safeObject = component;

            var meth = _safeObject.GetType().Method(methodParams[1]);
            if (meth != null)
            {
                Delegate del = Delegate.CreateDelegate(act.EventHandlerType, _safeObject, methodParams[1], false, false);
                
                if (del != null)
                    act.AddEventHandler(target, del);
                    
            }
            
        }
    }
}