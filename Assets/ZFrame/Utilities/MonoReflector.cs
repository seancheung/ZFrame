using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEngine;
using ZFrame.Utilities;
using Object = UnityEngine.Object;

public static class MonoReflector
{
    public static IEnumerable<MonoMember> GetMethods(this GameObject gameObject)
    {
        IEnumerable<Component> components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            Type type = component.GetType();
            IList<MethodInfo> methods = type.Methods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
            foreach (
                MethodInfo methodInfo in
                    methods.Where(
                        m =>
                            !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.ReturnType == typeof (void)))
                yield return new MonoMember(component, methodInfo);
        }

        foreach (
            MethodInfo methodInfo in
                typeof (GameObject).Methods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        m =>
                            !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.ReturnType == typeof (void)
                    ))
        {
            yield return new MonoMember(gameObject, methodInfo);
        }
    }

    public static IEnumerable<MonoMember> GetEvents(this GameObject gameObject)
    {
        IEnumerable<Component> components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            Type type = component.GetType();
            EventInfo[] events = type.GetEvents();
            foreach (EventInfo eventInfo in events)
                yield return new MonoMember(component, eventInfo);
        }

        foreach (EventInfo eventInfo in typeof (GameObject).GetEvents(BindingFlags.Public))
        {
            yield return new MonoMember(gameObject, eventInfo);
        }
    }

    [Serializable]
    public class MonoMember : ISerializationCallbackReceiver
    {
        private static string format = "{0}/{1}";
        private static string eventFormat = "{0}/{1}({2})";

        public Object component;
        public MemberInfo member;
        private string path;

        public MonoMember(Object component, MemberInfo member)
        {
            this.component = component;
            this.member = member;
        }

        public override string ToString()
        {
            if (member is EventInfo)
                return string.Format(eventFormat, component.GetType().Name(), member.Name,
                    member.To<EventInfo>().EventHandlerType.Name());
            return string.Format(format, component.GetType().Name(), member.Name);
        }

        public void OnBeforeSerialize()
        {
            if (member != null) path = member.ToString();
        }

        public void OnAfterDeserialize()
        {
            if (path != null && component)
            {
                string[] memberParams = path.Split(new[] {'/', '(', ')'});
                if (memberParams.Length == 2) //method
                {
                    member = memberParams[0] == typeof (GameObject).Name()
                        ? typeof (GameObject).GetMethod(memberParams[1])
                        : component.GetType().GetMethod(memberParams[1]);
                }
                else if (memberParams.Length == 3) //event
                {
                }
            }
        }
    }
}