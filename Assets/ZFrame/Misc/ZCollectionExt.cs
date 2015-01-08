using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ZFrame
{
	public static class ZCollectionExt
	{
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
			where TKey : class where TValue : class
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
	}
}