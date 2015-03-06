using System;
using System.Collections;
using UnityEngine;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVoter<TEnum> : MonoBehaviour where TEnum : IComparable, IConvertible
	{
		public delegate bool HandleVote(MonoVote<TEnum> vote);

		public delegate IEnumerator HandleVoteAsync(MonoVote<TEnum> vote, Action<bool> agreeCallback);

		public event HandleVote VoteHandler;

		private bool OnVoteHandler(MonoVote<TEnum> vote)
		{
			HandleVote handler = VoteHandler;
			if (handler != null) return handler(vote);
			return true;
		}

		public event HandleVoteAsync VoteAsyncHandler;

		private void OnVoteAsyncHandler(MonoVote<TEnum> vote, Action<bool> agreecallback)
		{
			HandleVoteAsync handler = VoteAsyncHandler;
			if (handler != null) StartCoroutine(handler(vote, agreecallback));
			else agreecallback.Invoke(true);
		}

		public bool Vote(MonoVote<TEnum> vote)
		{
			return MonoVoteEngine.Instance.Vote(vote);
		}

		public void VoteAsync(MonoVote<TEnum> vote, Action<bool> voteCallback)
		{
			MonoVoteEngine.Instance.VoteAsync(vote, voteCallback);
		}

		public bool Agree(MonoVote<TEnum> vote)
		{
			return OnVoteHandler(vote);
		}

		public void AgreeAsync(MonoVote<TEnum> vote, Action<bool> agreeCallback)
		{
			OnVoteAsyncHandler(vote, agreeCallback);
		}
	}
}