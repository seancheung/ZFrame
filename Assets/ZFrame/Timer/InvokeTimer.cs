using System;
using UnityEngine;

namespace ZFrame.Timer
{
	public class InvokeTimer : MonoSingleton<InvokeTimer>, ITimer, IMonoDisposable
	{
		public event Action Ontick;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }
		public bool IsRunning { get; protected set; }

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
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
			return false;
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