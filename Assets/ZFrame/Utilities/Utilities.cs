using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class Utilities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreCloneAttribute : Attribute
    {
    }

    /// <summary>
    /// Copy value
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="includeField"></param>
    /// <returns></returns>
    public static bool CloneTo<TSource, TTarget>(this TSource source, TTarget target, bool includeField = false)
        where TSource : class
        where TTarget : TSource
    {
        if (source == null || target == null)
        {
            Debug.LogError("argument NULL");
            return false;
        }

        foreach (PropertyInfo pi in source.GetType().GetProperties())
            if (pi.CanRead && pi.CanWrite && Attribute.GetCustomAttribute(pi, typeof (IgnoreCloneAttribute)) == null)
                pi.SetValue(target, pi.GetValue(source, null), null);

        if (includeField)
        {
            foreach (
                FieldInfo fi in
                    source.GetType()
                        .GetFields()
                        .Where(f => Attribute.GetCustomAttribute(f, typeof (IgnoreCloneAttribute)) == null))
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
    /// <param name="includeField"></param>
    /// <returns></returns>
    public static bool CloneFrom<TSource, TTarget>(this TTarget target, TSource source, bool includeField = false)
        where TSource : class
        where TTarget : TSource
    {
        return CloneTo(source, target, includeField);
    }

    /// <summary>
    /// Safely cast source object to target type
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TTarget As<TSource, TTarget>(this TSource source)
        where TSource : class
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

    public static IEnumerable<T> GetFlags<T>(this T target) where T : struct, IFormattable, IConvertible, IComparable
    {
        return Enum.GetValues(typeof (T)).Cast<T>().Where(value => Box<int>(value, target, (a, b) => (a & b) == a));
    }

    public static bool Box<T>(object value, object flags,
        Func<T, T, bool> op)
    {
        return op((T) value, (T) flags);
    }

    public static bool Between(this int num, int min, int max)
    {
        return num > min && num < max;
    }

    public static bool BetweenAnd(this int num, int min, int max)
    {
        return num >= min && num <= max;
    }

    public static bool TryAdd<T>(this IList<T> list, T item)
    {
        if (list.Contains(item)) return false;
        list.Add(item);
        return true;
    }

    /// <summary>
    /// Safe add item to list. Check if item is null or duplicate add
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool SafeAdd<T>(this IList<T> list, T item) where T : class
    {
        if (item == null)
            return false;
        return list.TryAdd(item);
    }

    /// <summary>
    /// Try remove
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool TryRemove<T>(this IList<T> list, T item)
    {
        if (!list.Contains(item)) return false;
        list.Remove(item);
        return true;
    }

    /// <summary>
    /// Safe remove item from list. Check if item is null or included
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool SafeRemove<T>(this IList<T> list, T item) where T : class
    {
        if (item == null) return false;
        return list.TryRemove(item);
    }

    /// <summary>
    /// Try add
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key)) return false;
        dict.Add(key, value);
        return true;
    }

    /// <summary>
    /// Safe add
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : class
        where TValue : class
    {
        if (key == null || value == null) return false;
        return dict.TryAdd(key, value);
    }

    /// <summary>
    /// Replace add
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// 
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void ReplaceAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key)) dict[key] = value;
        else dict.Add(key, value);
    }

    /// <summary>
    /// Try get
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        return dict.ContainsKey(key) ? dict[key] : default(TValue);
    }

    /// <summary>
    /// Try remove
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        if (!dict.ContainsKey(key)) return false;
        dict.Remove(key);
        return true;
    }

    /// <summary>
    /// Safe remove
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool SafeRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TKey : class
    {
        if (key == null) return false;
        return dict.TryRemove(key);
    }

    /// <summary>
    /// Convert to readonly collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> enumerable)
    {
        return new ReadOnlyCollection<T>(enumerable.ToList());
    }

    public static T At<T>(this IList<T> list, int index) where T : class
    {
        return list.Count > index && index >= 0 ? list[index] : null;
    }

    /// <summary>
    /// Change Target gameobject's layer
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <param name="changeChildren">change all children</param>
    public static void ChangeLayer(GameObject go, int layer, bool changeChildren = true)
    {
        go.layer = layer;
        if (!changeChildren)
        {
            return;
        }
        foreach (Transform child in go.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;
        }
    }

    public static void ChangeScale(this Transform target, float scale)
    {
        target.localScale = new Vector3(scale, scale, scale);
    }
}