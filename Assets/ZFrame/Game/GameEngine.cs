using System.Collections.Generic;
using ZFrame.Debugger;

namespace ZFrame
{
	public class GameEngine : MonoSingleton<GameEngine>, IZDisposable
	{
		private readonly List<IZDisposable> _disposables = new List<IZDisposable>();

		/// <summary>
		/// Register disposable class
		/// </summary>
		/// <param name="disposable"></param>
		public void RegisterDispose(IZDisposable disposable)
		{
			_disposables.SafeAdd(disposable);
		}

		private void Start()
		{
			RegisterDispose(this);
		}

		private void OnApplicationQuit()
		{
			foreach (IZDisposable disposable in _disposables)
			{
				if (disposable.DisposeOnApplicationQuit())
				{
					ZDebug.Log(disposable.GetType() + " has been disposed");
				}
			}
		}

		public void DisposeAll()
		{
			foreach (IZDisposable disposable in _disposables)
			{
				if (disposable.Dispose())
				{
					ZDebug.Log(disposable.GetType() + " has been disposed");
				}
			}
		}

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