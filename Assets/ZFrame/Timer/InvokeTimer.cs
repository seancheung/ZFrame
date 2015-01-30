using System;
using ZFrame.MonoBase;

namespace ZFrame.Timer
{
	public class InvokeTimer : MonoSingleton<InvokeTimer>, ITimer
	{
		public event Action Ontick;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }
		public bool IsRunning { get; protected set; }

		private void Start()
		{
			Sync(0);
			Sync(DateTime.Now);
		}

		protected virtual void Tick()
		{
			if (IsRunning)
			{
				Time++;
				Now = Now.AddSeconds(1);
				if (Ontick != null)
				{
					Ontick();
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
				CancelInvoke("Tick");
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				InvokeRepeating("Tick", 0, 1*UnityEngine.Time.timeScale);
			}
		}
	}
}