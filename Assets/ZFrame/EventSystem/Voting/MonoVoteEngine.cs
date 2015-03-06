using System;
using System.Collections.Generic;
using System.Linq;
using ZFrame.Base.MonoBase;

namespace ZFrame.EventSystem.Voting
{
	public class MonoVoteEngine : MonoSingleton<MonoVoteEngine>
	{
		private IEnumerable<MonoVoter<TEnum>> GetVoters<TEnum>() where TEnum : IComparable, IConvertible
		{
			return FindObjectsOfType<MonoVoter<TEnum>>();
		}

		internal bool Vote<TEnum>(MonoVote<TEnum> vote) where TEnum : IComparable, IConvertible
		{
			return GetVoters<TEnum>().Where(v => v != vote.EventArg.Sender).All(v => v.Agree(vote));
		}

		internal void VoteAsync<TEnum>(MonoVote<TEnum> vote, Action<bool> voteCallback)
			where TEnum : IComparable, IConvertible
		{
			List<bool> results = new List<bool>();
			foreach (MonoVoter<TEnum> voter in GetVoters<TEnum>().Where(v => v != vote.EventArg.Sender))
			{
				voter.AgreeAsync(vote, result =>
				{
					results.Add(result);
					if (results.Count >= GetVoters<TEnum>().Count(v => v != vote.EventArg.Sender))
						voteCallback.Invoke(results.All(r => r));
				});
			}
		}
	}
}