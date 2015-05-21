using System.Collections;
using UnityEngine;

public class MotionHandler
{
    public enum ParameterType
    {
        Time,
        Speed
    }

    public static float Translate(Transform target, Vector3 from, Vector3 to, float value,
        ParameterType type)
    {
        float rate = (type == ParameterType.Time) ? 1f/value : 1f/Vector3.Distance(from, to)*value;
        TranslateHandler handler = target.GetComponent<TranslateHandler>() ??
                                   target.gameObject.AddComponent<TranslateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.TranslateCoroutine(target, from, to, rate));
        return 1f/rate;
    }

    public static float Translate(Transform target, Vector3 to, float value,
        ParameterType type)
    {
        return Translate(target, target.position, to, value, type);
    }

    public static void Rotate(Transform target, Vector3 degrees, float value, ParameterType type)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.RotateCoroutine(target, degrees, value, type));
    }

    public static void Look(Transform target, Vector3 to, float value, ParameterType type)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.LookCoroutine(target, to, value, type));
    }

    public static void Clear(Transform target)
    {
        foreach (MotionHandlerBase component in target.GetComponents<MotionHandlerBase>())
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
        public IEnumerator RotateCoroutine(Transform target, Vector3 degrees, float value, ParameterType type)
        {
            Quaternion from = transform.rotation;
            Quaternion to = transform.rotation*Quaternion.Euler(degrees);
            float rate = (type == ParameterType.Time) ? 1f/value : 1f/Quaternion.Angle(from, to)*value;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Slerp(from, to, t);
                yield return new WaitForEndOfFrame();
            }
            Destroy(this);
        }

        public IEnumerator LookCoroutine(Transform target, Vector3 lookTarget, float value, ParameterType type)
        {
            Vector3 dir = lookTarget - target.position;
            dir.y = 0;
            Quaternion to = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion from = transform.rotation;
            float rate = (type == ParameterType.Time) ? 1f/value : 1f/Quaternion.Angle(from, to)*value;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Slerp(from, to, t);
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