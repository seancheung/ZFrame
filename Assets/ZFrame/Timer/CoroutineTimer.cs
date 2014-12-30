using System;
using System.Collections;
using UnityEngine;

namespace ZFrame.Timer
{
	public class CoroutineTimer : MonoSingleton<CoroutineTimer>, ITimer, IGameDisposable
	{
		public event TickCallback Ontick;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }
		public bool IsRunning { get; private set; }

		private IEnumerator Tick()
		{
			while (IsRunning)
			{
				Time++;
				Now = Now.AddSeconds(1);
				if (Ontick != null)
				{
					Ontick();
				}
				yield return new WaitForSeconds(1*UnityEngine.Time.timeScale);
			}
		}

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			Sync(0);
			Sync(DateTime.Now);
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
				StopCoroutine(Tick());
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				StartCoroutine(Tick());
			}
		}

		#region Test

		private void OnGUI()
		{
			GUILayout.Label(Time.ToString());
		}

		#endregion

		public bool DisposeOnApplicationQuit()
		{
			return false;
		}

		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}
	}
}