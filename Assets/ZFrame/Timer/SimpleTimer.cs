using System;
using System.Timers;
using UnityEngine;

namespace ZFrame.Timer
{
	public class SimpleTimer : MonoSingleton<SimpleTimer>, IGameDisposable
	{
		public event TickCallback Ontick;

		protected System.Timers.Timer timer;
		public ulong Time { get; protected set; }
		public DateTime Now { get; protected set; }

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			timer = new System.Timers.Timer(1000);
			timer.Start();
			timer.Elapsed += OnTick;
		}

		private void OnTick(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			Time++;
			Now = elapsedEventArgs.SignalTime;
			if (Ontick != null)
			{
				Ontick();
			}
		}

		public void Sync(ulong time)
		{
			Time = time;
		}

		private void OnGUI()
		{
			GUILayout.Label(Now.ToLongTimeString());
		}

		#region Implementation of IGameDisposable

		/// <summary>
		/// Called on application quit
		/// </summary>
		/// <returns></returns>
		public bool DisposeOnApplicationQuit()
		{
			timer.Close();
			return true;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <returns></returns>
		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}

		#endregion
	}
}