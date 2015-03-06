using System;
using UnityEngine;
using ZFrame.Base.MonoBase;

namespace ZFrame.Timer
{
	public sealed class InvokeTimer : MonoSingleton<InvokeTimer>, ITimer
	{
	    public float Interval { get; private set; }
	    public event Action Ontick;
		public ulong TickCount { get; private set; }
        public DateTime Now { get; private set; }
        public bool IsRunning { get; private set; }

		private void Start()
		{
			Sync(0);
			Sync(DateTime.Now);
		}

		private void Tick()
		{
			if (IsRunning)
			{
				TickCount++;
				Now = Now.AddSeconds(Interval);
				if (Ontick != null)
				{
					Ontick();
				}
			}
		}

		public void Sync(ulong time)
		{
			TickCount = time;
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
				CancelInvoke("Tick");
			}
		}

        public void StartTimer(float interval = 1f)
		{
            if (interval <= 0)
                throw new ArgumentException("Interval must be above 0");

			if (!IsRunning)
			{
                Interval = interval;
				IsRunning = true;
                InvokeRepeating("Tick", 0, Interval * Time.timeScale);
			}
		}
	}
}