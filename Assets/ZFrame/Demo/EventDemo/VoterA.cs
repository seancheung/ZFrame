using System;
using System.Collections;
using UnityEngine;
using ZFrame.EventSystem.Voting;

public class VoterA : MonoVoter<VoteEventType>
{
	private string _tip = "";

	private void Awake()
	{
		VoteHandler += OnVoteHandler;
		VoteAsyncHandler += OnVoteAsyncHandler;
	}

	private bool OnVoteHandler(MonoVote<VoteEventType> vote)
	{
		_tip = "";

		switch (vote.Key)
		{
			case VoteEventType.CreatureAttack:
				bool result = vote.EventArg.Data is VoterA;
				_tip = "my vote: " + result;
				return result;
		}

		_tip = "i don't care";
		return true;
	}

	private IEnumerator OnVoteAsyncHandler(MonoVote<VoteEventType> vote, Action<bool> agreeCallback)
	{
		_tip = "";

		switch (vote.Key)
		{
			case VoteEventType.CreatureAttack:
			{
				int count = 10;
				while (count-- > 0)
				{
					_tip = "thinking..." + count;
					yield return new WaitForSeconds(1);
				}

				_tip = "my vote: " + false;
				agreeCallback.Invoke(false);
			}
				yield break;
		}

		_tip = "i don't care";
		agreeCallback.Invoke(true);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(5, 5, 200, 50), "VoterA: CreatureAttack"))
		{
			bool result = GetComponent<MonoVoter<VoteEventType>>()
				.Vote(new MonoVote<VoteEventType>(VoteEventType.CreatureAttack,
					new MonoVoteArg<VoteEventType>(GetComponent<MonoVoter<VoteEventType>>(), FindObjectsOfType<VoterA>())));
			_tip = "total: " + result;
		}
		GUI.Label(new Rect(5, 60, 200, 50), _tip);
	}
}