using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZFrame.EventSystem
{
	public class MonoEventListenr : MonoBehaviour
	{
		private readonly Dictionary<string, Action<MonoEventArg>> _events = new Dictionary<string, Action<MonoEventArg>>();
		public bool isListening = true;

		public bool AddEvent(string key, Action<MonoEventArg> action)
		{
			if (!_events.SafeAdd(key, action)) return false;
			return true;
		}

		public bool RemoveEvent(string key)
		{
			if (!_events.SafeRemove(key))
				return false;
			return true;
		}

		public void ClearEvent()
		{
			_events.Clear();
		}

		public void HandleEvent(MonoEvent evt)
		{
			Action<MonoEventArg> action = _events.TryGet(evt.Name);
			if (action != null)
			{
				action.Invoke(evt.EventArg);
			}
		}

		private void OnDisable()
		{
			isListening = true;
		}

		private void OnEnable()
		{
			isListening = false;
		}
	}
}