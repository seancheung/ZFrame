using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ZFrame.Base.MonoBase
{
    public abstract class MonoManager<T> : MonoManager where T : MonoManager<T>
    {
        public static T Instance
        {
            get
            {
                if (!Application.isPlaying)
                    return null;

                T component = null;

                if (instance)
                    component = instance.GetComponent<T>();

                if (!instance || !component)
                    Debug.LogWarning("Instance has not been initialized or has been disposed. Type: " + typeof (T));

                return component;
            }
        }

        public static void Dispose()
        {
            Dispose<T>();
        }

        public static void Init()
        {
            Init<T>();
        }
    }


    public abstract class MonoManager : MonoBehaviour
    {
        protected static GameObject instance;

        public static void DisposeAll()
        {
            if (instance)
                Destroy(instance);
        }

        public static void InitAll()
        {
            foreach (
                Type type in
                    Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(
                            t =>
                                !t.IsAbstract && t.IsSubclassOf(typeof (MonoManager)) &&
                                Attribute.GetCustomAttribute(t, typeof (IgnoreOnInitAll)) == null))
            {
                CreateInstance(type);
            }
        }

        /// <summary>
        /// Call this to destroy manager
        /// </summary>
        protected static void Dispose<T>() where T : MonoManager
        {
            if (instance && instance.GetComponent<T>())
            {
                Destroy(instance.GetComponent<T>());
            }
        }

        /// <summary>
        /// Call this to initialize manager
        /// </summary>
        protected static void Init<T>() where T : MonoManager
        {
            CreateInstance(typeof (T));
        }

        private static void CreateInstance(Type type)
        {
            if (!instance)
            {
                instance = GameObject.FindGameObjectWithTag("MonoManager");
                instance = instance ?? new GameObject("MonoManagers");
                instance.tag = "MonoManager";
                instance.hideFlags = HideFlags.DontSave;
            }
            if (!instance.GetComponent(type))
            {
                instance.AddComponent(type).hideFlags = HideFlags.DontSave;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (instance)
                Destroy(instance);
        }
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class IgnoreOnInitAll : Attribute
{
}