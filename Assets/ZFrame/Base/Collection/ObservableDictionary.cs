using System;
using System.Collections.Generic;
using System.Linq;

namespace ZFrame.Collections.Observable
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public event Action<TKey, TValue> AddHanler;

		protected virtual void OnAddHanler(TKey arg1, TValue arg2)
		{
			Action<TKey, TValue> handler = AddHanler;
			if (handler != null) handler(arg1, arg2);
		}

		public event Action<TKey, TValue> RemoveHandler;

		protected virtual void OnRemoveHandler(TKey arg1, TValue arg2)
		{
			Action<TKey, TValue> handler = RemoveHandler;
			if (handler != null) handler(arg1, arg2);
		}

		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			OnAddHanler(key, value);
		}

		public new void Clear()
		{
			KeyValuePair<TKey, TValue>[] items = this.ToArray();
			base.Clear();
			foreach (KeyValuePair<TKey, TValue> item in items)
				OnRemoveHandler(item.Key, item.Value);
		}

		public new bool Remove(TKey key)
		{
			TValue value = base[key];
			bool result = base.Remove(key);
			if (result) OnRemoveHandler(key, value);
			return result;
		}
	}
}