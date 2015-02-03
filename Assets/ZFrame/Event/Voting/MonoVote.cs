using System;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVote<TEnum> : MonoEvent<TEnum> where TEnum : IComparable, IConvertible
	{
		public MonoVote(TEnum key, MonoVoteArg<TEnum> eventArg)
			: base(key, eventArg)
		{
		}

		public new MonoVoteArg<TEnum> EventArg
		{
			get { return (MonoVoteArg<TEnum>) base.EventArg; }
			set { base.EventArg = value; }
		}

		public static implicit operator TEnum(MonoVote<TEnum> vote)
		{
			return vote.Key;
		}
	}

	public class MonoVoteArg<TEnum> : MonoEventArg where TEnum : IComparable, IConvertible
	{
		public MonoVoteArg(MonoVoter<TEnum> sender, object data)
			: base(sender, data)
		{
		}

		public MonoVoteArg(MonoVoter<TEnum> sender)
			: base(sender)
		{
		}

		public new MonoVoter<TEnum> Sender
		{
			get { return (MonoVoter<TEnum>) base.Sender; }
			set { base.Sender = value; }
		}
	}
}