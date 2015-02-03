using UnityEngine;
using ZFrame.EventSystem.Voting;

public class VoterC : MonoVoter<VoteEventType>
{
	private string _tip = "";

	private void Awake()
	{
		VoteHandler += OnVoteHandler;
	}

	private bool OnVoteHandler(MonoVote<VoteEventType> vote)
	{
		_tip = "";

		switch (vote.Key)
		{
			case VoteEventType.CreatureAttack:
				bool result = vote.EventArg.Data is int && ((int) vote.EventArg.Data) > 5;
				_tip = "my vote: " + result;
				return result;
			case VoteEventType.CreatureBlock:
				_tip = "my vote: " + false;
				return false;
		}
		_tip = "i don't care";
		return true;
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(425, 5, 200, 50), "VoterC: CreatureAttack"))
		{
			bool result = GetComponent<MonoVoter<VoteEventType>>()
				.Vote(new MonoVote<VoteEventType>(VoteEventType.CreatureAttack,
					new MonoVoteArg<VoteEventType>(GetComponent<MonoVoter<VoteEventType>>(), FindObjectOfType<VoterA>())));
			_tip = "total: " + result;
		}
		if (GUI.Button(new Rect(425, 60, 200, 50), "VoterC: CreatureAttack(Async)"))
		{
			_tip = "waiting...";
			GetComponent<MonoVoter<VoteEventType>>()
				.VoteAsync(new MonoVote<VoteEventType>(VoteEventType.CreatureAttack,
					new MonoVoteArg<VoteEventType>(GetComponent<MonoVoter<VoteEventType>>(), FindObjectOfType<VoterA>())),
					OnReceivedVoteResult);
		}
		GUI.Label(new Rect(425, 115, 200, 50), _tip);
	}

	private void OnReceivedVoteResult(bool result)
	{
		_tip = "total: " + result;
	}
}