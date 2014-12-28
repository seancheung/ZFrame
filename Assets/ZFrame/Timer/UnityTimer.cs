using UnityEngine;

namespace ZFrame.Timer
{
	public class UnityTimer : MonoSingleton<UnityTimer>, IGameDisposable
	{
		public event TickCallback Ontick;

		public ulong Time { get; protected set; }

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
			InvokeRepeating("Tick", 0, 1);
		}

		private void Tick()
		{
			Time++;
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
			GUILayout.Label(Time.ToString());
		}

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
			ReleaseInstance();
			return true;
		}

		#endregion
	}
}