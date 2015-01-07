using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ZFrame
{
	public static class ZCollectionExt
	{
		/// <summary>
		/// Safe add item to list. Check if item is null or duplicate add
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool SafeAdd<T>(this IList<T> list, T item)
		{
			if (item != null && !list.Contains(item))
			{
				list.Add(item);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Safe remove item from list. Check if item is null or included
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="item"></param>
		public static void SafeRemove<T>(this IList<T> list, T item)
		{
			if (item != null && list.Contains(item))
			{
				list.Remove(item);
			}
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