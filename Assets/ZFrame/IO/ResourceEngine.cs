using System;
using System.Collections;
using UnityEngine;
using ZFrame.MonoBase;
using Object = UnityEngine.Object;

namespace ZFrame.IO
{
	public class ResourceEngine : MonoSingleton<ResourceEngine>
	{
		private const string Path = "ZResources";
		private ZResource _resource;

		private bool CanLoad
		{
			get
			{
				if (_resource != null)
					return true;
				return InitManager();
			}
		}

		private bool InitManager()
		{
			GameObject go = Resources.Load<GameObject>(Path);
			if (go != null)
				_resource = go.GetComponent<ZResource>();
			else
				Debug.LogError(Path + " not found!");

			return _resource != null;
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
				ZResource.Resource resource = Instance._resource[key];
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

            Debug.LogError("Resource converting failed: " + key + " --> " + typeof(T));

			return null;
		}

		/// <summary>
		/// Load and Instantiate a gameobject
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public GameObject LoadAndInstantiate(string key)
		{
			Object resource = Load(key);
			if (resource == null) return null;

			GameObject result = resource as GameObject;
			if (result != null)
				return Instantiate(result) as GameObject;
			return null;
		}

		/// <summary>
		/// Instantiate async using coroutine
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="onInstatiated"></param>
		public void InstantiateAsync<T>(string key, Action<T> onInstatiated) where T : Object
		{
			StartCoroutine(LoadAndInstantiate(key, onInstatiated));
		}

		private IEnumerator Load<T>(string key, Action<T> onLoaded) where T : Object
		{
			T obj = Load<T>(key);
			yield return obj;
			if (onLoaded != null)
			{
				onLoaded.Invoke(obj);
			}
		}

		private IEnumerator LoadAndInstantiate<T>(string key, Action<T> onInstatiated) where T : Object
		{
			GameObject obj = LoadAndInstantiate(key);
			yield return obj;
			if (onInstatiated != null)
			{
				onInstatiated.Invoke(obj as T);
			}
		}

		#endregion

		private void Start()
		{
			InitManager();
		}

	}
}