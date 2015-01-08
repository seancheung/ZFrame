using System;

namespace ZFrame.Timer
{
	public interface ITimer
	{
		event Action Ontick;
		ulong Time { get; }
		DateTime Now { get; }
		bool IsRunning { get; }
		void Sync(ulong time);
		void Sync(DateTime time);
		void StopTimer();
		void StartTimer();
	}
}