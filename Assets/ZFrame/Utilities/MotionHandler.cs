using System.Collections;
using UnityEngine;

public class MotionHandler
{
    public enum TranslateType
    {
        Time,
        Speed
    }

    public static float Translate(Transform target, Vector3 from, Vector3 to, float value,
        TranslateType type)
    {
        float rate = (type == TranslateType.Time) ? 1f/value : 1f/Vector3.Distance(from, to)*value;
        TranslateHandler handler = target.GetComponent<TranslateHandler>() ??
                                   target.gameObject.AddComponent<TranslateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.TranslateCoroutine(target, from, to, rate));
        return 1f/rate;
    }

    public static float Translate(Transform target, Vector3 to, float value,
        TranslateType type)
    {
        return Translate(target, target.position, to, value, type);
    }

    public static void Rotate(Transform target, Vector3 degrees, float time)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.RotateCoroutine(target, degrees, time));
    }

    public static void Look(Transform target, Vector3 to, float time)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.LookCoroutine(target, to, time));
    }

    public static void Clear(Transform target)
    {
        foreach (var component in target.GetComponents<MotionHandlerBase>())
        {
            Object.Destroy(component);
        }
    }

    private class TranslateHandler : MotionHandlerBase
    {
        public IEnumerator TranslateCoroutine(Transform target, Vector3 from, Vector3 to, float rate)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.position = Vector3.Lerp(from, to, t);
                yield return new WaitForEndOfFrame();
            }
            Destroy(this);
        }
    }

    private class RotateHandler : MotionHandlerBase
    {
        public IEnumerator RotateCoroutine(Transform target, Vector3 degrees, float time)
        {
            Quaternion from = transform.rotation;
            Quaternion to = transform.rotation*Quaternion.Euler(degrees);
            float rate = 1f/time;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Lerp(from, to, t);
                yield return new WaitForEndOfFrame();
            }
            Destroy(this);
        }

        public IEnumerator LookCoroutine(Transform target, Vector3 to, float time)
        {
            Vector3 dir = to - target.position;
            dir.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion from = transform.rotation;
            float rate = 1f/time;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Lerp(from, quaternion, t);
                yield return new WaitForEndOfFrame();
            }
            Destroy(this);
        }
    }

    private class MotionHandlerBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }
    }
}