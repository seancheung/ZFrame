namespace ZFrame.EventSystem.Voting
{
	public class MonoVote : MonoEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MonoEvent"/> class.
		/// </summary>
		public MonoVote(string name, MonoEventArg eventArg)
			: base(name, eventArg)
		{
		}
	}
}