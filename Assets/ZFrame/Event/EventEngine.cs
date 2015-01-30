using System.Collections.Generic;
using ZFrame.Debugger;
using ZFrame.MonoBase;

namespace ZFrame.EventSystem
{
	public class EventEngine : MonoSingleton<EventEngine>
	{
		private readonly Queue<IEvent> _events = new Queue<IEvent>();
		private readonly Dictionary<string, List<IEventListener>> _listeners = new Dictionary<string, List<IEventListener>>();

		/// <summary>
		/// Listen to target event
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="eventName"></param>
		/// <returns></returns>
		public bool AddListener(IEventListener listener, string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return false;
			}
			if (!_listeners.ContainsKey(eventName))
			{
				_listeners.Add(eventName, new List<IEventListener>());
			}
			return _listeners[eventName].SafeAdd(listener);
		}

		/// <summary>
		/// Stop listen to target event
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="eventName"></param>
		public void RemoveListener(IEventListener listener, string eventName)
		{
			if (!string.IsNullOrEmpty(eventName) && _listeners.ContainsKey(eventName))
			{
				_listeners[eventName].SafeRemove(listener);
			}
		}

		/// <summary>
		/// Add event to queue
		/// </summary>
		/// <param name="evt"></param>
		public void QueueEvent(IEvent evt)
		{
			lock (this)
			{
				_events.Enqueue(evt);
			}
		}

		private void Dispatch()
		{
			if (_events.Count > 0)
			{
				IEvent evt = _events.Dequeue();
				if (!_listeners.ContainsKey(evt.Name))
				{
					ZDebug.LogError("Event " + evt.Name + " has no listeners");
					return;
				}

				for (int i = 0; i < _listeners[evt.Name].Count; i++)
				{
					IEventListener listener = _listeners[evt.Name][i];
					if (listener == null)
					{
						_listeners[evt.Name].RemoveAt(i);
					}
					else
					{
						listener.HandleEvent(evt);
					}
				}
			}
		}

		private void Update()
		{
			Dispatch();
		}
	}
}