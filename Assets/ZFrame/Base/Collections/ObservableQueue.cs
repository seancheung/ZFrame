using System;
using System.Collections.Generic;

namespace ZFrame.Base.Collections.Observable
{
	public class ObservableQueue<T> : Queue<T>
	{
		public ObservableQueue()
		{
		}

		public ObservableQueue(int capacity) : base(capacity)
		{
		}

		public ObservableQueue(IEnumerable<T> collection) : base(collection)
		{
		}

		#region Observable Handler

		public event Action<T> EnqueHandler;

		protected virtual void OnEnqueHandler(T obj)
		{
			Action<T> handler = EnqueHandler;
			if (handler != null) handler(obj);
		}

		public event Action<T> DequeHandler;

		protected virtual void OnDequeHandler(T obj)
		{
			Action<T> handler = DequeHandler;
			if (handler != null) handler(obj);
		}

		#endregion


		public new T Dequeue()
		{
			T item = base.Dequeue();
			OnDequeHandler(item);
			return item;
		}

		public new void Enqueue(T item)
		{
			base.Enqueue(item);
			OnEnqueHandler(item);
		}

		public new void Clear()
		{
			T[] items = ToArray();
			base.Clear();
			foreach (T item in items)
				OnDequeHandler(item);
		}
	}

}