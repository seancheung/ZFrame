using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZFrame.MonoBase;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVoteEngine : MonoSingleton<MonoVoteEngine>
	{
		private IEnumerable<MonoVoter> Voters
		{
			get { return FindObjectsOfType<MonoVoter>(); }
		}

		internal bool Vote(MonoVote vote)
		{
			return Voters.All(v => v.Agree(vote));
		}

		internal void VoteAsync(MonoVote vote, Action<bool> voteCallback)
		{
			StartCoroutine(GetVote(vote, voteCallback));
		}

		private IEnumerator GetVote(MonoVote vote, Action<bool> callback)
		{
			foreach (MonoVoter voter in Voters)
			{
				if (!voter.Agree(vote))
				{
					callback.Invoke(false);
					yield break;
				}
				yield return voter;
			}
			callback.Invoke(true);
		}
	}
}