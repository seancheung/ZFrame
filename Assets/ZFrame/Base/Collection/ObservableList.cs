using System;
using System.Collections.Generic;

namespace ZFrame.Collections.Observable
{
	public class ObservableList<T> : List<T>
	{
		public event Action<T> AddHandler;

		protected virtual void OnAddHandler(T obj)
		{
			Action<T> handler = AddHandler;
			if (handler != null) handler(obj);
		}

		public event Action<T> RemoveHandler;

		protected virtual void OnRemoveHandler(T obj)
		{
			Action<T> handler = RemoveHandler;
			if (handler != null) handler(obj);
		}

		public new void Add(T item)
		{
			base.Add(item);
			OnAddHandler(item);
		}

		public new void AddRange(IEnumerable<T> collection)
		{
			base.AddRange(collection);
			foreach (T item in collection)
				OnAddHandler(item);
		}

		public new void Insert(int index, T item)
		{
			base.Insert(index, item);
			OnAddHandler(item);
		}

		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			base.InsertRange(index, collection);
			foreach (T item in collection)
				OnAddHandler(item);
		}

		public new bool Remove(T item)
		{
			bool result = base.Remove(item);
			if (result) OnRemoveHandler(item);
			return result;
		}

		public new int RemoveAll(Predicate<T> match)
		{
			List<T> items = FindAll(match);
			int count = base.RemoveAll(match);
			foreach (T item in items)
				OnRemoveHandler(item);
			return count;
		}

		public new void RemoveAt(int index)
		{
			T item = base[index];
			base.RemoveAt(index);
			OnRemoveHandler(item);
		}

		public new void RemoveRange(int index, int count)
		{
			List<T> items = GetRange(index, count);
			base.RemoveRange(index, count);
			foreach (T item in items)
				OnRemoveHandler(item);
		}

		public new void Clear()
		{
			T[] items = ToArray();
			base.Clear();
			foreach (T item in items)
				OnRemoveHandler(item);
		}
	}
}