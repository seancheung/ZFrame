using System;
using System.Collections;
using UnityEngine;
using ZFrame.Base.MonoBase;

namespace ZFrame.Timer
{
    public sealed class CoroutineTimer : MonoSingleton<CoroutineTimer>, ITimer
    {
        public float Interval { get; private set; }
        public event Action Ontick;
        public ulong TickCount { get; private set; }
        public DateTime Now { get; private set; }
        public bool IsRunning { get; private set; }

        private IEnumerator Tick()
        {
            while (IsRunning)
            {
                TickCount++;
                Now = Now.AddSeconds(Interval);
                if (Ontick != null)
                {
                    Ontick();
                }
                yield return new WaitForSeconds(Interval*Time.timeScale);
            }
        }

        private void Start()
        {
            Sync(0);
            Sync(DateTime.Now);
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
                StopCoroutine(Tick());
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
                StartCoroutine(Tick());
            }
        }
    }
}