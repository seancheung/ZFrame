using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZFrame.Timer
{
	public class ThreadTimer : MonoSingleton<ThreadTimer>, ITimer, IZDisposable
	{
		protected System.Threading.Timer timer;
		public event TickCallback Ontick;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }
		public bool IsRunning { get; protected set; }

		internal List<Clock> clocks = new List<Clock>();
		internal List<Reminder> reminders = new List<Reminder>();

		protected virtual void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			Sync(0);
			Sync(DateTime.Now);
		}

		protected virtual void OnTick(object state)
		{
			if (IsRunning)
			{
				Time++;
				Now = Now.AddSeconds(1);

				if (Ontick != null)
				{
					Ontick();
				}

				clocks.ForEach(c => c.Update());
				for (int i = 0; i < reminders.Count; i++)
				{
					if (reminders[i].Update(Now) && reminders[i].Type == ReminderType.OneTime)
					{
						reminders.RemoveAt(i);
					}
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

		public void StopTimer()
		{
			if (IsRunning)
			{
				IsRunning = false;
				if (timer != null)
				{
					timer.Dispose();
				}
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				timer = new System.Threading.Timer(OnTick, null, 0, 1000);
			}
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

		#region Test

		private void OnGUI()
		{
			GUILayout.Label(Time.ToString());
		}

		#endregion

		#region Implementation of IGameDisposable

		/// <summary>
		/// Called on application quit
		/// </summary>
		/// <returns></returns>
		public bool DisposeOnApplicationQuit()
		{
			StopTimer();
			return true;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <returns></returns>
		public bool Dispose()
		{
			StopTimer();
			ReleaseInstance();
			return true;
		}

		#endregion
	}
}