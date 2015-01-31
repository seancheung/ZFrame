using System;
using System.Collections;
using UnityEngine;
using ZFrame.EventSystem.Voting;

[RequireComponent(typeof (MonoVoter))]
public class VoterB : MonoBehaviour
{
	private string _tip = "";

	private void Awake()
	{
		GetComponent<MonoVoter>().VoteHandler += OnVoteHandler;
		GetComponent<MonoVoter>().VoteAsyncHandler += OnVoteAsyncHandler;
	}

	private bool OnVoteHandler(MonoVote vote)
	{
		_tip = "";

		switch (vote.Type)
		{
			case MonoEventType.CreatureBlock:
				_tip = "my vote: " + false;
				return false;
		}

		_tip = "i don't care";
		return true;
	}

	private IEnumerator OnVoteAsyncHandler(MonoVote vote, Action<bool> agreeCallback)
	{
		_tip = "";

		switch (vote.Type)
		{
			case MonoEventType.CreatureAttack:
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
			bool result = GetComponent<MonoVoter>()
				.Vote(new MonoVote(MonoEventType.CreatureBlock,
					new MonoVoteArg(GetComponent<MonoVoter>(), FindObjectsOfType<VoterA>())));
			_tip = "total: " + result;
		}
		GUI.Label(new Rect(210, 60, 200, 50), _tip);
	}
}