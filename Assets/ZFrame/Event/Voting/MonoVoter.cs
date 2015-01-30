using System;
using UnityEngine;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVoter : MonoBehaviour
	{
		public delegate bool HandleVote(MonoVote vote);

		public event HandleVote VoteHandler;

		protected virtual bool OnVoteHandler(MonoVote vote)
		{
			HandleVote handler = VoteHandler;
			if (handler != null) return handler(vote);
			return true;
		}

		public void Vote(MonoVote vote)
		{
			MonoVoteEngine.Instance.Vote(vote);
		}

		public void VoteAsync(MonoVote vote, Action<bool> voteCallback)
		{
			MonoVoteEngine.Instance.VoteAsync(vote, voteCallback);
		}

		internal bool Agree(MonoVote vote)
		{
			return OnVoteHandler(vote);
		}
	}
}