using System;
using System.Collections.Generic;

namespace ZFrame.Base.Collections.Observable
{
	public class ObservableStack<T> : Stack<T>
	{
		public ObservableStack()
		{
		}

		public ObservableStack(int capacity)
			: base(capacity)
		{
		}

		public ObservableStack(IEnumerable<T> collection)
			: base(collection)
		{
		}

		#region Observable Handler

		public event Action<T> PopHandler;

		protected virtual void OnPopHandler(T obj)
		{
			Action<T> handler = PopHandler;
			if (handler != null) handler(obj);
		}

		public event Action<T> PushHandler;

		protected virtual void OnPushHandler(T obj)
		{
			Action<T> handler = PushHandler;
			if (handler != null) handler(obj);
		}

		#endregion

		public new T Pop()
		{
			T item = base.Pop();
			OnPopHandler(item);
			return item;
		}

		public new void Push(T item)
		{
			base.Push(item);
			OnPushHandler(item);
		}

		public new void Clear()
		{
			T[] items = ToArray();
			base.Clear();
			foreach (T item in items)
				OnPopHandler(item);
		}
	}
}