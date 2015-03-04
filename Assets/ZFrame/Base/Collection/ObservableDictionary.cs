using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ZFrame.Collections.Observable
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public ObservableDictionary()
		{
		}

		public ObservableDictionary(int capacity)
			: base(capacity)
		{
		}

		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
			: base(capacity, comparer)
		{
		}

		public ObservableDictionary(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: base(dictionary, comparer)
		{
		}

		protected ObservableDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#region Observable Handler

		public event Action<TKey, TValue> AddHandler;

		protected virtual void OnAddHandler(TKey key, TValue value)
		{
			Action<TKey, TValue> handler = AddHandler;
			if (handler != null) handler(key, value);
		}

		public event Action<TKey, TValue> RemoveHandler;

		protected virtual void OnRemoveHandler(TKey key, TValue value)
		{
			Action<TKey, TValue> handler = RemoveHandler;
			if (handler != null) handler(key, value);
		}

		public event Action<TKey, TValue> ChangedHandler;

		protected virtual void OnChangedHandler(TKey key, TValue value)
		{
			Action<TKey, TValue> handler = ChangedHandler;
			if (handler != null) handler(key, value);
		}

		#endregion

		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			OnAddHandler(key, value);
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

		public new TValue this[TKey key]
		{
			get { return base[key]; }
			set
			{
				base[key] = value;
				OnChangedHandler(key, value);
			}
		}
	}
}