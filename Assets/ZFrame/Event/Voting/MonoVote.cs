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

}