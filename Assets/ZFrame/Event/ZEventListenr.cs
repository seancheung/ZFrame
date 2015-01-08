using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZFrame.Event
{
	public class ZEventListenr : MonoBehaviour, IEventListener
	{
		private readonly Dictionary<string, Action<object>> _events = new Dictionary<string, Action<object>>();
		public bool stopHandle;

		public bool AddEvent(string key, Action<object> action)
		{
			if (!_events.SafeAdd(key, action)) return false;
			EventEngine.Instance.AddListener(this, key);
			return true;
		}

		public bool RemoveEvent(string key)
		{
			if (!_events.SafeRemove(key))
				return false;
			EventEngine.Instance.RemoveListener(this, key);
			return true;
		}

		public void ClearEvent()
		{
			_events.Clear();
		}

		#region Implementation of IEventListener

		public void HandleEvent(IEvent evt)
		{
			if (!stopHandle)
			{
				Action<object> action = _events.TryGet(evt.Name);
				if (action != null)
				{
					action.Invoke(evt.Data);
				}
			}
		}

		#endregion

		private void OnDisable()
		{
			stopHandle = true;
		}

		private void OnEnable()
		{
			stopHandle = false;
		}

		private void OnDestroy()
		{
			_events.Keys.ToList().ForEach(key => EventEngine.Instance.RemoveListener(this, key));
		}
	}
}