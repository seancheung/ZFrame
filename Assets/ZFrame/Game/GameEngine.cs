using System.Collections.Generic;
using UnityEngine;

namespace ZFrame
{
	public class GameEngine : MonoSingleton<GameEngine>, IGameDisposable
	{
		private readonly List<IGameDisposable> _disposables = new List<IGameDisposable>();

		/// <summary>
		/// Register disposable class
		/// </summary>
		/// <param name="disposable"></param>
		public void RegisterDispose(IGameDisposable disposable)
		{
			_disposables.SafeAdd(disposable);
		}

		private void Start()
		{
			RegisterDispose(this);
		}

		private void OnApplicationQuit()
		{
			foreach (IGameDisposable disposable in _disposables)
			{
				if (disposable.DisposeOnApplicationQuit())
				{
					Debug.Log(disposable.GetType() + " has been disposed");
				}
			}
		}

		public void DisposeAll()
		{
			foreach (IGameDisposable disposable in _disposables)
			{
				if (disposable.Dispose())
				{
					Debug.Log(disposable.GetType() + " has been disposed");
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