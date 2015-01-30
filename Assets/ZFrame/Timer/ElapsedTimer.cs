using System;
using System.Timers;
using ZFrame.MonoBase;

namespace ZFrame.Timer
{
	public class ElapsedTimer : MonoSingleton<ElapsedTimer>, ITimer
	{
		public event Action Ontick;
		protected System.Timers.Timer timer;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }
		public bool IsRunning { get; private set; }

		private void Start()
		{
			Sync(0);
			Sync(DateTime.Now);
		}

		private void OnTick(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			if (IsRunning)
			{
				Time++;
				Now = Now.AddSeconds(1);
				Now = elapsedEventArgs.SignalTime;
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
				if (timer != null)
				{
					timer.Close();
				}
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				timer = new System.Timers.Timer(1000);
				timer.Elapsed += OnTick;
				timer.Start();
			}
		}
	}
}