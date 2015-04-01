using System.Reflection;
using UnityEngine;

namespace ZFrame.Utilities
{
    public static class ZClassTool
    {
        /// <summary>
        /// Copy value
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="copyField"></param>
        /// <returns></returns>
        public static bool CopyValueTo<TSource, TTarget>(this TSource source, TTarget target, bool copyField = false)
            where TSource : class
            where TTarget : TSource
        {
            if (source == null || target == null)
            {
                Debug.LogError("argument NULL");
                return false;
            }

            foreach (PropertyInfo pi in source.GetType().GetProperties())
                if (pi.CanRead && pi.CanWrite)
                    pi.SetValue(target, pi.GetValue(source, null), null);

            if (copyField)
            {
                foreach (FieldInfo fi in source.GetType().GetFields())
                    fi.SetValue(target, fi.GetValue(source));
            }

            return true;
        }

        /// <summary>
        /// Copy value
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="copyField"></param>
        /// <returns></returns>
        public static bool CopyValueFrom<TSource, TTarget>(this TTarget target, TSource source, bool copyField = false)
            where TSource : class
            where TTarget : TSource
        {
            return CopyValueTo(source, target, copyField);
        }

        /// <summary>
        /// Safely cast source object to target type
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget As<TSource, TTarget>(this TSource source) where TSource : class
            where TTarget : class
        {
            return source as TTarget;
        }

        /// <summary>
        /// Cast source object to target type
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget To<TTarget>(this object source)
            where TTarget : class
        {
            return (TTarget) source;
        }

        public static bool Between(this int num, int min, int max)
        {
            return num > min && num < max;
        }

        public static bool BetweenAnd(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }
    }
}