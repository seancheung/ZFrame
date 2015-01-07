using ZFrame.Debugger;

public class GameFSM
{
	public IGameState CurrentState { get; private set; }

	public void StateChange(IGameState newState)
	{
		if (newState == null)
		{
			ZDebug.LogError("State error!");
			return;
		}
		if (CurrentState != null)
		{
			CurrentState.OnExit();
		}

		CurrentState = newState;

		CurrentState.OnEnter();
	}

	public void Update()
	{
		if (CurrentState != null)
		{
			CurrentState.OnStay();
		}
	}
}