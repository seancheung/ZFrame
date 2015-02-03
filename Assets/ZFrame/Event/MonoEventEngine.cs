using System;
using System.Collections.Generic;
using System.Linq;
using ZFrame.Collections.Observable;
using ZFrame.MonoBase;

namespace ZFrame.EventSystem
{
	public class MonoEventEngine<TEnum> : MonoSingleton<MonoEventEngine<TEnum>>
		where TEnum : IComparable, IConvertible
	{
		private readonly ObservableQueue<MonoEvent<TEnum>> _events = new ObservableQueue<MonoEvent<TEnum>>();

		private IEnumerable<MonoEventListenr<TEnum>> Listeners
		{
			get { return FindObjectsOfType<MonoEventListenr<TEnum>>(); }
		}

		public void QueueEvent(MonoEvent<TEnum> evt)
		{
			_events.Enqueue(evt);
		}

		protected virtual void Start()
		{
			_events.DequeHandler += Dispatch;
		}

		protected virtual void Update()
		{
			if (_events.Count > 0)
				_events.Dequeue();
		}

		private void Dispatch(MonoEvent<TEnum> monoEvent)
		{
			foreach (MonoEventListenr<TEnum> listener in Listeners.Where(l => l.isListening))
			{
				listener.HandleEvent(monoEvent);
			}
		}
	}
}