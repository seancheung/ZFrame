using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame.MonoBase
{
	public abstract class MonoSingleton<T> : SingleMono where T : MonoBehaviour
	{
		///// <summary>
		///// A readonly instance
		///// </summary>
		//protected static readonly T Instance = new GameObject(typeof (T).ToString(), typeof (T)).GetComponent<T>();

		private static GameObject _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					T find = FindObjectOfType<T>();
					_instance = find == null ? new GameObject(typeof (T).ToString(), typeof (T)) : find.gameObject;
					DontDestroyOnLoad(_instance);
				}
				return _instance.GetComponent<T>();
			}
		}

		/// <summary>
		/// Destroy instance gameobject immediately
		/// </summary>
		protected virtual void ReleaseInstance()
		{
			Destroy(_instance);
		}

		/// <summary>
		/// Force instance to be initialized
		/// </summary>
		public virtual void Init()
		{
			ZDebug.Log(typeof (T) + " is ready");
		}
	}
}