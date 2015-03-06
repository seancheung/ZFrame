using System;

namespace ZFrame.Timer
{
    public interface ITimer
    {
        float Interval { get; }
        event Action Ontick;
        ulong TickCount { get; }
        DateTime Now { get; }
        bool IsRunning { get; }
        void Sync(ulong time);
        void Sync(DateTime time);
        void StopTimer();
        void StartTimer(float interval = 1f);
    }
}