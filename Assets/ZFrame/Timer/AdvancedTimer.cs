using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZFrame.Timer
{
	public class AdvancedTimer : MonoSingleton<AdvancedTimer>, IGameDisposable
	{
		protected System.Threading.Timer timer;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }

		internal List<Clock> clocks = new List<Clock>();
		internal List<Reminder> reminders = new List<Reminder>();

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			timer = new System.Threading.Timer(OnTick, null, 0, 1000);
			Sync(0);
			Sync(DateTime.Now);
		}

		private void OnTick(object state)
		{
			Time++;
			Now = Now.AddSeconds(1);

			clocks.ForEach(c => c.Update());
			for (int i = 0; i < reminders.Count; i++)
			{
				if (reminders[i].Update(Now))
				{
					reminders.RemoveAt(i);
				}
			}
		}

		public void Sync(ulong time)
		{
			Time = time;
		}

		public void Sync(DateTime time)
		{
			Now = time;
		}

		public bool AddClock(string key, int interval, TickCallback callback)
		{
			if (string.IsNullOrEmpty(key) || clocks.Any(c => c.Key == key))
			{
				return false;
			}

			return clocks.SafeAdd(new Clock(key, interval, callback));
		}

		public void RemoveClock(string key)
		{
			clocks.RemoveAll(clock => clock.Key == key);
		}

		public bool AddReminder(string key, ReminderType type, DateTime time, TickCallback callback)
		{
			if (string.IsNullOrEmpty(key) || reminders.Any(c => c.Key == key))
			{
				return false;
			}

			return reminders.SafeAdd(new Reminder(key, type, time, callback));
		}

		private void OnGUI()
		{
			GUILayout.Label(Time.ToString());
		}

		#region Implementation of IGameDisposable

		/// <summary>
		/// Called on application quit
		/// </summary>
		/// <returns></returns>
		public bool DisposeOnApplicationQuit()
		{
			timer.Dispose();
			return true;
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