using System;
using System.Collections;
using UnityEngine;
using ZFrame.EventSystem.Voting;

public class VoterB : MonoVoter<VoteEventType>
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
			case VoteEventType.CreatureBlock:
				_tip = "my vote: " + false;
				return false;
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
				int count = 5;
				while (count-- > 0)
				{
					_tip = "thinking..." + count;
					yield return new WaitForSeconds(1);
				}
				_tip = "my vote: " + true;
				agreeCallback.Invoke(true);
			}
				yield break;
		}

		_tip = "i don't care";
		agreeCallback.Invoke(true);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(210, 5, 200, 50), "VoterB: CreatureBlock"))
		{
			bool result = GetComponent<MonoVoter<VoteEventType>>()
				.Vote(new MonoVote<VoteEventType>(VoteEventType.CreatureBlock,
					new MonoVoteArg<VoteEventType>(GetComponent<MonoVoter<VoteEventType>>(), FindObjectsOfType<VoterA>())));
			_tip = "total: " + result;
		}
		GUI.Label(new Rect(210, 60, 200, 50), _tip);
	}
}