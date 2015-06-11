using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MotionHandler
{
    [Serializable]
    public enum ParameterType
    {
        Time,
        Speed
    }

    [Serializable]
    public enum CurveType
    {
        Linear,
        QuadraticIn,
        QuadraticOut,
        QuadraticInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuarticIn,
        QuarticOut,
        QuarticInOut,
        QuinticIn,
        QuinticOut,
        QuinticInOut,
        SexticIn,
        SexticOut,
        SexticInOut,
        SepticIn,
        SepticOut,
        SepticInOut,
        SineIn,
        SineOut,
        SineInOut,
        Spring
    }

    public static float Translate(Transform target, Vector3 to, float value, ParameterType type,
        CurveType curve = CurveType.Linear)
    {
        return Translate(target, target.position, to, value, type, curve);
    }

    public static float Translate(Transform target, Vector3 from, Vector3 to, float value, ParameterType type,
        CurveType curve)
    {
        TranslateHandler handler = target.GetComponent<TranslateHandler>() ??
                                   target.gameObject.AddComponent<TranslateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.TranslateCoroutine(target, from, to, value, type, curve));
        return 1/handler.rate;
    }

    public static float Rotate(Transform target, Vector3 delta, float value, ParameterType type)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.RotateCoroutine(target, delta, value, type));
        return 1/handler.rate;
    }

    public static float Rotate(Transform target, Vector3 to, float value, ParameterType type,
        CurveType curve)
    {
        return Rotate(target, target.rotation, Quaternion.Euler(to), value, type, curve);
    }

    public static float Rotate(Transform target, Vector3 from, Vector3 to, float value, ParameterType type,
        CurveType curve)
    {
        return Rotate(target, Quaternion.Euler(from), Quaternion.Euler(to), value, type, curve);
    }

    public static float Rotate(Transform target, Quaternion to, float value, ParameterType type,
        CurveType curve)
    {
        return Rotate(target, target.rotation, to, value, type, curve);
    }

    public static float Rotate(Transform target, Quaternion from, Quaternion to, float value, ParameterType type,
        CurveType curve)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.RotateCoroutine(target, from, to, value, type, curve));
        return 1/handler.rate;
    }

    public static float Look(Transform target, Vector3 to, Vector3 upwards, float value, ParameterType type,
        CurveType curve = CurveType.Linear)
    {
        RotateHandler handler = target.GetComponent<RotateHandler>() ?? target.gameObject.AddComponent<RotateHandler>();
        handler.StopAllCoroutines();
        handler.StartCoroutine(handler.LookCoroutine(target, to, upwards, value, type, curve));
        return 1/handler.rate;
    }

    public static void Clear(Transform target)
    {
        foreach (MotionHandlerBase component in target.GetComponents<MotionHandlerBase>())
        {
            Object.Destroy(component);
        }
    }

    public static void ChangeParameter(Transform target, float value)
    {
        foreach (MotionHandlerBase handler in target.GetComponents<MotionHandlerBase>())
        {
            handler.parameter = value;
        }
    }

    private class TranslateHandler : MotionHandlerBase
    {
        public IEnumerator TranslateCoroutine(Transform target, Vector3 from, Vector3 to, float value,
            ParameterType type, CurveType curve)
        {
            CalculateRate(from, to, value, type);
            Func<float, float, float, float> func = CurveLerp(curve);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.position = Vector3.Lerp(from, to, func(0f, 1f, t));
                yield return new WaitForEndOfFrame();
                if (parameter != value)
                    CalculateRate(from, to, parameter, type);
            }
            Destroy(this);
        }
    }

    private class RotateHandler : MotionHandlerBase
    {
        public IEnumerator RotateCoroutine(Transform target, Vector3 delta, float value, ParameterType type)
        {
            CalculateRate(value, type);
            float speed = (type == ParameterType.Speed) ? parameter : 1;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = transform.rotation*Quaternion.Euler(delta*speed);
                yield return new WaitForEndOfFrame();
                if (parameter != value)
                {
                    CalculateRate(parameter, type);
                    speed = (type == ParameterType.Speed) ? parameter : 1;
                }
            }
            Destroy(this);
        }

        public IEnumerator LookCoroutine(Transform target, Vector3 lookTarget, Vector3 upwards, float value,
            ParameterType type,
            CurveType curve)
        {
            Vector3 dir = lookTarget - target.position;
            Quaternion to = Quaternion.LookRotation(dir, upwards);
            Quaternion from = transform.rotation;
            CalculateRate(from, to, value, type);
            Func<float, float, float, float> func = CurveLerp(curve);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Slerp(from, to, func(0f, 1f, t));
                yield return new WaitForEndOfFrame();
                if (parameter != value)
                    CalculateRate(from, to, parameter, type);
            }
            Destroy(this);
        }

        public IEnumerator RotateCoroutine(Transform target, Quaternion from, Quaternion to, float value,
            ParameterType type, CurveType curve)
        {
            CalculateRate(from, to, value, type);
            Func<float, float, float, float> func = CurveLerp(curve);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*rate;
                target.rotation = Quaternion.Slerp(from, to, func(0f, 1f, t));
                yield return new WaitForEndOfFrame();
                if (parameter != value)
                    CalculateRate(from, to, parameter, type);
            }
            Destroy(this);
        }
    }

    private abstract class MotionHandlerBase : MonoBehaviour
    {
        public float parameter;
        public float rate;

        protected void CalculateRate(float value, ParameterType type)
        {
            parameter = value;
            rate = (type == ParameterType.Time) ? 1f/value : 1f/float.PositiveInfinity;
        }

        protected void CalculateRate(Vector3 from, Vector3 to, float value, ParameterType type)
        {
            parameter = value;
            rate = (type == ParameterType.Time) ? 1f/value : 1f/Vector3.Distance(from, to)*value;
        }

        protected void CalculateRate(Quaternion from, Quaternion to, float value, ParameterType type)
        {
            parameter = value;
            rate = (type == ParameterType.Time) ? 1f/value : 1f/Quaternion.Angle(from, to)*value;
        }

        protected virtual void Awake()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }

        protected static Func<float, float, float, float> CurveLerp(CurveType curve)
        {
            switch (curve)
            {
                case CurveType.Linear:
                    return Linear;
                case CurveType.QuadraticIn:
                    return QuadraticIn;
                case CurveType.QuadraticOut:
                    return QuadraticOut;
                case CurveType.QuadraticInOut:
                    return QuadraticInOut;
                case CurveType.CubicIn:
                    return CubicIn;
                case CurveType.CubicOut:
                    return CubicOut;
                case CurveType.CubicInOut:
                    return CubicInOut;
                case CurveType.QuarticIn:
                    return QuarticIn;
                case CurveType.QuarticOut:
                    return QuarticOut;
                case CurveType.QuarticInOut:
                    return QuarticInOut;
                case CurveType.QuinticIn:
                    return QuinticIn;
                case CurveType.QuinticOut:
                    return QuinticOut;
                case CurveType.QuinticInOut:
                    return QuinticInOut;
                case CurveType.SexticIn:
                    return SexticIn;
                case CurveType.SexticOut:
                    return SexticOut;
                case CurveType.SexticInOut:
                    return SexticInOut;
                case CurveType.SepticIn:
                    return SepticIn;
                case CurveType.SepticOut:
                    return SepticOut;
                case CurveType.SepticInOut:
                    return SepticInOut;
                case CurveType.SineIn:
                    return SineIn;
                case CurveType.SineOut:
                    return SineOut;
                case CurveType.SineInOut:
                    return SineInOut;
                case CurveType.Spring:
                    return Spring;
                default:
                    throw new ArgumentOutOfRangeException("curve");
            }
        }

        private static float Linear(float a, float b, float f)
        {
            return Mathf.Lerp(a, b, f);
        }

        private static float QuadraticIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 2) + a;
        }

        private static float QuadraticOut(float a, float b, float f)
        {
            b -= a;
            return -b*f*(f - 2) + a;
        }

        private static float QuadraticInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 2) + a;
            f--;
            return -b/2f*(f*(f - 2f) - 1f) + a;
        }

        private static float CubicIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 3) + a;
        }

        private static float CubicOut(float a, float b, float f)
        {
            f--;
            b -= a;
            return b*Mathf.Pow(f, 3) + b + a;
        }

        private static float CubicInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 3) + a;
            f -= 2f;
            return b/2f*Mathf.Pow(f, 3) + b + a;
        }

        private static float QuarticIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 4) + a;
        }

        private static float QuarticOut(float a, float b, float f)
        {
            f--;
            b -= a;
            return -b*Mathf.Pow(f, 4) + b + a;
        }

        private static float QuarticInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 4) + a;
            f -= 2;
            return -b/2f*Mathf.Pow(f, 4) + b + a;
        }

        private static float QuinticIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 5) + a;
        }

        private static float QuinticOut(float a, float b, float f)
        {
            f--;
            b -= a;
            return b*Mathf.Pow(f, 5) + b + a;
        }

        private static float QuinticInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 5) + a;
            f -= 2;
            return b/2f*Mathf.Pow(f, 5) + b + a;
        }

        private static float SexticIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 6) + a;
        }

        private static float SexticOut(float a, float b, float f)
        {
            f--;
            b -= a;
            return -b*Mathf.Pow(f, 6) + b + a;
        }

        private static float SexticInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 6) + a;
            f -= 2;
            return -b/2f*Mathf.Pow(f, 6) + b + a;
        }

        private static float SepticIn(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Pow(f, 7) + a;
        }

        private static float SepticOut(float a, float b, float f)
        {
            f--;
            b -= a;
            return b*Mathf.Pow(f, 7) + b + a;
        }

        private static float SepticInOut(float a, float b, float f)
        {
            f *= 2f;
            b -= a;
            if (f < 1) return b/2f*Mathf.Pow(f, 7) + a;
            f -= 2f;
            return b/2f*Mathf.Pow(f, 7) + b + a;
        }

        private static float SineIn(float a, float b, float f)
        {
            b -= a;
            return -b*Mathf.Cos(f*(Mathf.PI*0.5f)) + b + a;
        }

        private static float SineOut(float a, float b, float f)
        {
            b -= a;
            return b*Mathf.Sin(f*(Mathf.PI*0.5f)) + a;
        }

        private static float SineInOut(float a, float b, float f)
        {
            b -= a;
            return -b*0.5f*(Mathf.Cos(Mathf.PI*f) - 1) + a;
        }

        private static float Spring(float a, float b, float f)
        {
            f = Mathf.Clamp01(f);
            f = (Mathf.Sin(f*Mathf.PI*(0.2f + 2.5f*Mathf.Pow(f, 3)))*Mathf.Pow(1f - f, 2.2f) + f)*(1f + (1.2f*(1f - f)));
            return a + (b - a)*f;
        }
    }
}