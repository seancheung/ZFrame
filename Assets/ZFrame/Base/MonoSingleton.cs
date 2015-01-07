using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
					_instance = new GameObject(typeof (T).ToString(), typeof (T));
					DontDestroyOnLoad(_instance);
				}
				return _instance.GetComponent<T>();
			}
		}

		/// <summary>
		/// Destroy instance gameobject immediately
		/// </summary>
		protected void ReleaseInstance()
		{
			DestroyImmediate(_instance);
		}

		/// <summary>
		/// Force instance to be initialized
		/// </summary>
		public void Init()
		{
			ZDebug.Log(typeof (T) + " is ready");
		}
	}
}