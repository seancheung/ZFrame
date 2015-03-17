using System;
using System.Collections;
using UnityEngine;
using ZFrame.Utilities;
using Object = UnityEngine.Object;

namespace ZFrame.IO.ResourceSystem
{
    public sealed class ResourcePool : MonoStatic<ResourcePool>
    {
        public static string path = "ResourceAsset";
        private static ResourceAsset _asset;

        private void Awake()
        {
            if (!_asset)
                _asset = Resources.Load<ResourceAsset>(path);
        }

        private Object Load(string groupName, string key)
        {
            ResourceAsset.Group group = _asset.groups.Find(g => g.groupName == groupName);
            var res = group.resources.Find(r => r.resourceKey == key);
            if(res == null)return null;

            switch (res.type)
            {
                case ResourceAsset.ResourceType.Reference:
                    return res.resource;
                case ResourceAsset.ResourceType.PathLink:
                    return Resources.Load(res.path.Replace("Assets/Resources/", ""));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Object Load(string filter)
        {
            string[] strs = filter.Split('/');
            if (strs.Length != 2)
                throw new ArgumentException("filter must be 'groupName/key'");
            return Instance.Load(strs[0], strs[1]);
        }

        public static T Load<T>(string filter) where T : Object
        {
            return Load(filter).To<T>();
        }

        public static void LoadAsync<T>(string filter, Action<T> loadedCallback) where T : Object
        {
            Instance.StartCoroutine(Instance.LoadCoroutine(filter, loadedCallback));
        }

        public static T Instantiate<T>(string filter) where T : Object
        {
            T obj = Load<T>(filter);
            return Instantiate(obj);
        }

        public static void InstantiateAsync<T>(string filter, Action<T> instantiatedCallback) where T : Object
        {
            Instance.StartCoroutine(Instance.InstantiateCoroutine(filter, instantiatedCallback));
        }

        private IEnumerator InstantiateCoroutine<T>(string filter, Action<T> instantiatedCallback) where T : Object
        {
            T original = Load<T>(filter);
            yield return original;
            T obj = Instantiate(original);
            yield return obj;
            instantiatedCallback.Invoke(obj);
        }

        private IEnumerator LoadCoroutine<T>(string filter, Action<T> loadedCallback) where T : Object
        {
            T obj = Load<T>(filter);
            loadedCallback.Invoke(obj);
            yield return obj;
        }

        private IEnumerator LoadAsset(string url, Action<AssetBundle> callback)
        {
            WWW www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                callback.Invoke(www.assetBundle);
                www.Dispose();
            }
        }
    }
}