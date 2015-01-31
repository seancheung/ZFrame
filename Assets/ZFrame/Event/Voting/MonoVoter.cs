using System;
using System.Collections;
using UnityEngine;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVoter : MonoBehaviour
	{
		public delegate bool HandleVote(MonoVote vote);

		public delegate IEnumerator HandleVoteAsync(MonoVote vote, Action<bool> agreeCallback);

		public event HandleVote VoteHandler;

		protected virtual bool OnVoteHandler(MonoVote vote)
		{
			HandleVote handler = VoteHandler;
			if (handler != null) return handler(vote);
			return true;
		}

		public event HandleVoteAsync VoteAsyncHandler;

		protected virtual void OnVoteAsyncHandler(MonoVote vote, Action<bool> agreecallback)
		{
			HandleVoteAsync handler = VoteAsyncHandler;
			if (handler != null) StartCoroutine(handler(vote, agreecallback));
			else agreecallback.Invoke(true);
		}

		public bool Vote(MonoVote vote)
		{
			return MonoVoteEngine.Instance.Vote(vote);
		}

		public void VoteAsync(MonoVote vote, Action<bool> voteCallback)
		{
			MonoVoteEngine.Instance.VoteAsync(vote, voteCallback);
		}

		public bool Agree(MonoVote vote)
		{
			return OnVoteHandler(vote);
		}

		public void AgreeAsync(MonoVote vote, Action<bool> agreeCallback)
		{
			OnVoteAsyncHandler(vote, agreeCallback);
		}
	}
}