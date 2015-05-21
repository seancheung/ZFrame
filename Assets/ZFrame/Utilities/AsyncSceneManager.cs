using System;
using System.Collections;
using UnityEngine;

public class AsyncSceneManager
{
    public static void LoadLevel(string level, Action<float> loadingCallback)
    {
        GameObject go = new GameObject("LoadLevel_" + level);
        SceneLoader manager = go.AddComponent<SceneLoader>();
        manager.StartCoroutine(manager.Load(level, loadingCallback));
    }

    public static void LoadLevel(int level, Action<float> loadingCallback)
    {
        GameObject go = new GameObject("LoadLevel_" + level);
        SceneLoader manager = go.AddComponent<SceneLoader>();
        manager.StartCoroutine(manager.Load(level, loadingCallback));
    }


    private class SceneLoader : MonoBehaviour
    {
        private AsyncOperation async;
        private Action<float> callback;

        private void Awake()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        public IEnumerator Load(string level, Action<float> loadingCallback)
        {
            callback = loadingCallback;
            async = Application.LoadLevelAsync(level);
            while (!async.isDone)
            {
                if (async != null)
                {
                    if (callback != null) callback.Invoke(async.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }

        public IEnumerator Load(int level, Action<float> loadingCallback)
        {
            callback = loadingCallback;
            async = Application.LoadLevelAsync(level);
            while (!async.isDone)
            {
                if (async != null)
                {
                    if (callback != null) callback.Invoke(async.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }

        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}