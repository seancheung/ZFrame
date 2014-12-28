namespace ZFrame
{
	/// <summary>
	/// For game class dispose
	/// </summary>
	public interface IGameDisposable
	{
		/// <summary>
		/// Called on application quit
		/// </summary>
		/// <returns></returns>
		bool DisposeOnApplicationQuit();

		/// <summary>
		/// Dispose
		/// </summary>
		/// <returns></returns>
		bool Dispose();

	}

}