namespace ZFrame.Net.Message
{
	public class MessageEngine : MonoSingleton<MessageEngine>, IGameDisposable
	{
		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
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