using System;
using UnityEngine;

namespace ZFrame.EventSystem
{
	public class MonoEventListenr<TEnum> : MonoBehaviour where TEnum : IComparable, IConvertible
	{
		public bool isListening = true;

		public event Action<MonoEvent<TEnum>> EventHandler;

		private void OnEventHandler(MonoEvent<TEnum> obj)
		{
			Action<MonoEvent<TEnum>> handler = EventHandler;
			if (handler != null) handler(obj);
		}

		public void HandleEvent(MonoEvent<TEnum> evt)
		{
			OnEventHandler(evt);
		}

		protected virtual void OnDisable()
		{
			isListening = false;
		}

		protected virtual void OnEnable()
		{
			isListening = true;
		}
		
	}

	public class MonoEventListener : MonoBehaviour
	{
		public bool isListening = true;

		public event Action<MonoEvent> EventHandler;

		protected virtual void OnEventHandler(MonoEvent obj)
		{
			Action<MonoEvent> handler = EventHandler;
			if (handler != null) handler(obj);
		}

		public void HandleEvent(MonoEvent evt)
		{
			OnEventHandler(evt);
		}

		protected virtual void OnDisable()
		{
			isListening = false;
		}

		protected virtual void OnEnable()
		{
			isListening = true;
		}
	}
}