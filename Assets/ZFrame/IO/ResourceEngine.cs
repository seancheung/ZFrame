using UnityEngine;

namespace ZFrame.IO
{
	public class ResourceEngine : MonoSingleton<ResourceEngine>, IGameDisposable
	{
		private const string Path = "ResourceReferences";
		private ResourceRef _resourceRef;

		private bool CanLoad
		{
			get { return _resourceRef != null; }
		}

		private void InitManager()
		{
			GameObject go = Resources.Load<GameObject>(Path);
			if (go != null)
			{
				_resourceRef = go.GetComponent<ResourceRef>();
			}
		}

		#region Load by key

		/// <summary>
		/// Load resource by key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Object Load(string key)
		{
			if (CanLoad)
			{
				ResourceRef.GameResource resource = Instance._resourceRef[key];
				if (resource != null && resource.resource != null) return resource.resource;

				Debug.LogError("Resource not found: " + key);
				return null;
			}

			Debug.LogError("Resource cannot be loaded: " + key);
			return null;
		}

		/// <summary>
		/// Load resource by key
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Load<T>(string key) where T : Object
		{
			Object resource = Load(key);
			if (resource == null) return null;

			T result = resource as T;
			if (result != null)
				return result;

			Debug.LogError("Resource converting failed: " + key + " --> " + typeof (T));

			return null;
		}

		#endregion

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			InitManager();
		}

		public bool DisposeOnApplicationQuit()
		{
			return false;
		}

		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}
	}
}