using System;

namespace ZFrame.EventSystem.Voting
{
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