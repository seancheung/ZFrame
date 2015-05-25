using UnityEngine;

namespace ZFrame.Base.MonoBase
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
                    _instance.hideFlags = HideFlags.DontSave;
                }
                return _instance.GetComponent<T>() ?? _instance.AddComponent<T>();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (_instance)
                Destroy(_instance);
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