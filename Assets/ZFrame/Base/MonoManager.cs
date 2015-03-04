using UnityEngine;

namespace ZFrame.MonoBase
{
    public abstract class MonoManager<T> : MonoManager where T : MonoManager<T>
    {
        private static GameObject _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindGameObjectWithTag("MonoManager");
                    _instance = _instance ?? new GameObject("MonoManagers");
                    _instance.tag = "MonoManager";
                    DontDestroyOnLoad(_instance);
                }
                return _instance.GetComponent<T>() ?? _instance.AddComponent<T>();
            }
        }
    }


    public abstract class MonoManager : MonoBehaviour
    {
        /// <summary>
        /// Call this to destroy manager
        /// </summary>
        public virtual void Dispose()
        {
            Destroy(this);
        }

        /// <summary>
        /// Call this to initialize manager
        /// </summary>
        public virtual void Init()
        {
        }
    }
}