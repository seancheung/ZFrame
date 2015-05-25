using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ComponentLoader
{
    public static void Load<T>(this GameObject gameObject) where T : class
    {
        foreach (
            Type type in
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => !t.IsAbstract && typeof (T).IsAssignableFrom(t)))
        {
            gameObject.AddComponent(type);
        }
    }

    public static void Load<T>(this Component component) where T : class
    {
        Load<T>(component.gameObject);
    }
}