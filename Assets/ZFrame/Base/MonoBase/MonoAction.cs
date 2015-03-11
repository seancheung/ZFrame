using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class MonoAction : ISerializationCallbackReceiver
{
    [SerializeField] private Component component;

    private Action action;

    public string method;

    public void OnBeforeSerialize()
    {
        if (action != null)
            method = action.Method.Name;
    }

    public void OnAfterDeserialize()
    {
        if (component && component)
            action = (Action) CreateDelegate<Action>(this);
    }

    public static Delegate CreateDelegate(Type type, MonoAction monoAction)
    {
        if (monoAction.component == null || string.IsNullOrEmpty(monoAction.method))
        {
            return null;
        }

        return Delegate.CreateDelegate(type, monoAction.component, monoAction.method, false, false);
    }

    public static Delegate CreateDelegate<T>(MonoAction monoAction) where T : ICloneable, ISerializable
    {
        return CreateDelegate(typeof (T), monoAction);
    }

    public override string ToString()
    {
        return string.Format("Component: {0}, Method: {1}", component, method);
    }
}