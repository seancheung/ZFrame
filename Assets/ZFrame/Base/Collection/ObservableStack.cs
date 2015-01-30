using System;
using System.Collections.Generic;

namespace ZFrame.Collections.Observable
{
	public class ObservableStack<T> : Stack<T>
	{
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