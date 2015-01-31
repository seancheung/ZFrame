using System;
using UnityEngine;

namespace ZFrame.EventSystem
{
	public class MonoEventListenr : MonoBehaviour
	{
		public event Action<MonoEvent> EventHandler;

		protected virtual void OnEventHandler(MonoEvent obj)
		{
			Action<MonoEvent> handler = EventHandler;
			if (handler != null) handler(obj);
		}


		public bool isListening = true;

		public void HandleEvent(MonoEvent evt)
		{
			OnEventHandler(evt);
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