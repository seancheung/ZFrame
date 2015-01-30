using System.Collections.Generic;
using System.Linq;
using ZFrame.Collections.Observable;
using ZFrame.MonoBase;

namespace ZFrame.EventSystem
{
	public class MonoEventEngine : MonoSingleton<MonoEventEngine>
	{
		private readonly ObservableQueue<MonoEvent> _events = new ObservableQueue<MonoEvent>();

		private IEnumerable<MonoEventListenr> Listeners
		{
			get { return FindObjectsOfType<MonoEventListenr>(); }
		}

		public void QueueEvent(MonoEvent evt)
		{
			_events.Enqueue(evt);
		}

		private void Start()
		{
			_events.DequeHandler += Dispatch;
		}

		private void Update()
		{
			if (_events.Count > 0)
				_events.Dequeue();
		}

		private void Dispatch(MonoEvent monoEvent)
		{
			foreach (MonoEventListenr listener in Listeners.Where(l => l.isListening))
			{
				listener.HandleEvent(monoEvent);
			}
		}
	}
}