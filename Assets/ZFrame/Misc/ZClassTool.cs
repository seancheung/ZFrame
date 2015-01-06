using System.Reflection;
using UnityEngine;

namespace ZFrame
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
	}

}