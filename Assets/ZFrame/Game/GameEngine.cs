using System.Collections.Generic;
using ZFrame.Debugger;

namespace ZFrame
{
	public class GameEngine : MonoSingleton<GameEngine>, IMonoDisposable
	{
		private readonly List<IMonoDisposable> _disposables = new List<IMonoDisposable>();

		/// <summary>
		/// Register disposable class
		/// </summary>
		/// <param name="disposable"></param>
		public void RegisterDispose(IMonoDisposable disposable)
		{
			_disposables.SafeAdd(disposable);
		}

		private void Start()
		{
			RegisterDispose(this);
		}

		private void OnApplicationQuit()
		{
			foreach (IMonoDisposable disposable in _disposables)
			{
				if (disposable.DisposeOnApplicationQuit())
				{
					ZDebug.Log(disposable.GetType() + " has been disposed");
				}
			}
		}

		public void DisposeAll()
		{
			foreach (IMonoDisposable disposable in _disposables)
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