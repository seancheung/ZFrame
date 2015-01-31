namespace ZFrame.EventSystem.Voting
{
	public class MonoVote : MonoEvent
	{
		public MonoVote(MonoEventType type, MonoVoteArg eventArg)
			: base(type, eventArg)
		{
		}

		public new MonoVoteArg EventArg
		{
			get { return (MonoVoteArg) base.EventArg; }
			set { base.EventArg = value; }
		}

		public static implicit operator MonoEventType(MonoVote vote)
		{
			return vote.Type;
		}

	}

	public class MonoVoteArg : MonoEventArg
	{
		public MonoVoteArg(MonoVoter sender, object data) : base(sender, data)
		{
		}

		public MonoVoteArg(MonoVoter sender)
			: base(sender)
		{
		}

		public new MonoVoter Sender
		{
			get { return (MonoVoter) base.Sender; }
			set { base.Sender = value; }
		}
	}
}