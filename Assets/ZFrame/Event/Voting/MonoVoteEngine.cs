using System;
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
			return Voters.Where(v => v != vote.EventArg.Sender).All(v => v.Agree(vote));
		}

		internal void VoteAsync(MonoVote vote, Action<bool> voteCallback)
		{
			List<bool> results = new List<bool>();
			foreach (MonoVoter voter in Voters.Where(v => v != vote.EventArg.Sender))
			{
				voter.AgreeAsync(vote, result =>
				{
					results.Add(result);
					if (results.Count >= Voters.Count(v => v != vote.EventArg.Sender))
						voteCallback.Invoke(results.All(r => r));
				});
			}
		}
	}
}