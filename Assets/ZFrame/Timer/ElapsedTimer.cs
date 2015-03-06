using System;
using System.Timers;
using ZFrame.Base.MonoBase;

namespace ZFrame.Timer
{
    public sealed class ElapsedTimer : MonoSingleton<ElapsedTimer>, ITimer
    {
        public float Interval { get; private set; }
        public event Action Ontick;
        private System.Timers.Timer _timer;
        public ulong TickCount { get; private set; }
        public DateTime Now { get; private set; }
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
                TickCount++;
                Now = Now.AddSeconds(Interval);
                Now = elapsedEventArgs.SignalTime;
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
                if (_timer != null)
                {
                    _timer.Close();
                }
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
                _timer = new System.Timers.Timer(Interval*1000);
                _timer.Elapsed += OnTick;
                _timer.Start();
            }
        }
    }
}