using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame.MonoBase
{
	public abstract class MonoSingleton<T> : SingleMono where T : MonoSingleton<T>
	{
		private static GameObject _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					T find = FindObjectOfType<T>();
					_instance = find == null ? new GameObject(typeof (T).Name, typeof (T)) : find.gameObject;
					DontDestroyOnLoad(_instance);
				}
				return _instance.GetComponent<T>();
			}
		}

		/// <summary>
		/// Destroy instance gameobject
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

	public abstract class MonoSingleton<T, TMono>
		where T : MonoSingleton<T, TMono>, new()
		where TMono : DelegateMono
	{
		private static T _instance;
		private TMono _mono;

		public static T Instance
		{
			get { return _instance ?? (_instance = new T()); }
			protected set { _instance = value; }
		}

		protected TMono Mono
		{
			get
			{
				return _mono ??
				       (_mono =
					       new GameObject(string.Format("{0}({1})", typeof (T).Name, typeof (TMono).Name), typeof (TMono))
						       .GetComponent<TMono>());
			}
		}

		/// <summary>
		/// Destroy instance gameobject
		/// </summary>
		protected virtual void ReleaseInstance()
		{
			Object.Destroy(_mono.gameObject);
			_instance = null;
		}
	}
}