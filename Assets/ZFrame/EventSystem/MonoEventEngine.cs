using System;
using System.Collections.Generic;
using System.Linq;
using ZFrame.Base.Collections.Observable;
using ZFrame.Base.MonoBase;
using Object = UnityEngine.Object;

namespace ZFrame.EventSystem
{
	public class MonoEventEngine<TEnum> : MonoSingleton<MonoEventEngine<TEnum>, DelegateMonoMini>
		where TEnum : IComparable, IConvertible
	{
		public MonoEventEngine()
		{
			Mono.StartHandler += Start;
			Mono.UpdateHandler += Update;
		}

		private readonly ObservableQueue<MonoEvent<TEnum>> _events = new ObservableQueue<MonoEvent<TEnum>>();

		private IEnumerable<MonoEventListenr<TEnum>> Listeners
		{
			get { return Object.FindObjectsOfType<MonoEventListenr<TEnum>>(); }
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

	public class MonoEventEngine : MonoSingleton<MonoEventEngine>
	{
		private readonly ObservableQueue<MonoEvent> _events = new ObservableQueue<MonoEvent>();

		private IEnumerable<MonoEventListener> Listeners
		{
			get { return FindObjectsOfType<MonoEventListener>(); }
		}

		public void QueEvent(MonoEvent evt)
		{
			_events.Enqueue(evt);
		}

		protected virtual void Update()
		{
			if (_events.Count > 0)
				_events.Dequeue();
		}

		protected virtual void Start()
		{
			_events.DequeHandler += Dispatch;
		}

		private void Dispatch(MonoEvent monoEvent)
		{
			foreach (MonoEventListener listener in Listeners.Where(l => l.isListening))
			{
				listener.HandleEvent(monoEvent);
			}
		}
	}
}