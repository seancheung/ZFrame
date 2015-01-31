using UnityEngine;
using ZFrame.EventSystem.Voting;

[RequireComponent(typeof (MonoVoter))]
public class VoterC : MonoBehaviour
{
	private string _tip = "";

	private void Awake()
	{
		GetComponent<MonoVoter>().VoteHandler += OnVoteHandler;
	}

	private bool OnVoteHandler(MonoVote vote)
	{
		_tip = "";

		switch (vote.Type)
		{
			case MonoEventType.CreatureAttack:
				bool result = vote.EventArg.Data is int && ((int) vote.EventArg.Data) > 5;
				_tip = "my vote: " + result;
				return result;
			case MonoEventType.CreatureBlock:
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
			bool result = GetComponent<MonoVoter>()
				.Vote(new MonoVote(MonoEventType.CreatureAttack,
					new MonoVoteArg(GetComponent<MonoVoter>(), FindObjectOfType<VoterA>())));
			_tip = "total: " + result;
		}
		if (GUI.Button(new Rect(425, 60, 200, 50), "VoterC: CreatureAttack(Async)"))
		{
			_tip = "waiting...";
			GetComponent<MonoVoter>()
				.VoteAsync(new MonoVote(MonoEventType.CreatureAttack,
					new MonoVoteArg(GetComponent<MonoVoter>(), FindObjectOfType<VoterA>())), OnReceivedVoteResult);
		}
		GUI.Label(new Rect(425, 115, 200, 50), _tip);
	}

	private void OnReceivedVoteResult(bool result)
	{
		_tip = "total: " + result;
	}
}