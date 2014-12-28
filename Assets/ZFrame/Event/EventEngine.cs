using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrame.Event
{
	public class EventEngine : MonoSingleton<EventEngine>, IGameDisposable
	{
		private readonly Queue _events = Queue.Synchronized(new Queue());
		private readonly Dictionary<string, List<IEventListener>> _listeners = new Dictionary<string, List<IEventListener>>();

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
		}

		/// <summary>
		/// Listen to target event
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="eventName"></param>
		/// <returns></returns>
		public bool Listen(IEventListener listener, string eventName)
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
		public void StopListen(IEventListener listener, string eventName)
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
				IEvent evt = _events.Dequeue() as IEvent;
				if (!_listeners.ContainsKey(evt.Name))
				{
					Debug.LogError("Event " + evt.Name + " has no listeners");
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

		#region Implementation of IGameDisposable

		/// <summary>
		/// Called on application quit
		/// </summary>
		/// <returns></returns>
		public bool DisposeOnApplicationQuit()
		{
			return false;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <returns></returns>
		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}

		#endregion
	}
}