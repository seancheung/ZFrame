using System;
using ZFrame.Base.MonoBase;

namespace ZFrame.Timer
{
    public sealed class ThreadTimer : MonoSingleton<ThreadTimer>, ITimer
    {
        private System.Threading.Timer _timer;
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

        private void OnTick(object state)
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
                if (_timer != null)
                {
                    _timer.Dispose();
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
                _timer = new System.Threading.Timer(OnTick, null, 0, (int) (Interval*1000));
            }
        }
    }
}