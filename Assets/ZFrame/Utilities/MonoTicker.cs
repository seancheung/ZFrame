using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public static class MonoTicker
{
    public static void Run(float time, Action onStart, Action onTick, Action onComplete)
    {
        TickExecute execute =
            new GameObject("TickExecute") {hideFlags = HideFlags.HideAndDontSave}.AddComponent<TickExecute>();
        execute.StartCoroutine(RunCoroutine(execute.gameObject, time, onStart, onTick, onComplete));
    }

    public static void Run(float time, Action onStart, Action onTick, Action onComplete, float tickRate)
    {
        TickExecute execute =
            new GameObject("TickExecute") {hideFlags = HideFlags.HideAndDontSave}.AddComponent<TickExecute>();
        execute.StartCoroutine(RunCoroutine(execute.gameObject, time, onStart, onTick, onComplete, tickRate));
    }

    public static void Run(float time, Action onTick, float tickRate)
    {
        Run(time, null, onTick, null, tickRate);
    }

    public static void Run(float time, Action<float> onTick, float tickRate)
    {
        TickExecute execute =
            new GameObject("TickExecute") {hideFlags = HideFlags.HideAndDontSave}.AddComponent<TickExecute>();
        execute.StartCoroutine(RunCoroutine(execute.gameObject, time, null, onTick, null, tickRate));
    }

    public static void Run(float time, Action onTick)
    {
        Run(time, null, onTick, null);
    }

    public static void Run(float time, Action<float> onTick)
    {
        TickExecute execute =
            new GameObject("TickExecute") {hideFlags = HideFlags.HideAndDontSave}.AddComponent<TickExecute>();
        execute.StartCoroutine(RunCoroutine(execute.gameObject, time, null, onTick, null));
    }

    public static void Stop(GameObject go)
    {
        foreach (TickExecute component in go.GetComponents<TickExecute>())
        {
            Object.Destroy(component);
        }
    }

    public static void StopAll()
    {
        foreach (TickExecute component in Object.FindObjectsOfType<TickExecute>())
        {
            Object.Destroy(component);
        }
    }

    private static IEnumerator RunCoroutine(GameObject go, float time, Action onStart, Action onTick, Action onComplete,
        float tickRate)
    {
        if (onStart != null) onStart.Invoke();

        while (time > 0)
        {
            if (onTick != null) onTick.Invoke();

            time -= tickRate;
            yield return new WaitForSeconds(tickRate);
        }

        if (onComplete != null) onComplete.Invoke();
        Object.Destroy(go);
    }

    private static IEnumerator RunCoroutine(GameObject go, float time, Action onStart, Action onTick, Action onComplete)
    {
        if (onStart != null) onStart.Invoke();

        while (time > 0)
        {
            if (onTick != null) onTick.Invoke();

            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null) onComplete.Invoke();
        Object.Destroy(go);
    }

    private static IEnumerator RunCoroutine(GameObject go, float time, Action onStart, Action<float> onTick,
        Action onComplete,
        float tickRate)
    {
        if (onStart != null) onStart.Invoke();

        while (time > 0)
        {
            if (onTick != null) onTick.Invoke(time);

            time -= tickRate;
            yield return new WaitForSeconds(tickRate);
        }

        if (onComplete != null) onComplete.Invoke();
        Object.Destroy(go);
    }

    private static IEnumerator RunCoroutine(GameObject go, float time, Action onStart, Action<float> onTick,
        Action onComplete)
    {
        if (onStart != null) onStart.Invoke();

        while (time > 0)
        {
            if (onTick != null) onTick.Invoke(time);

            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null) onComplete.Invoke();
        Object.Destroy(go);
    }

    private class TickExecute : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}