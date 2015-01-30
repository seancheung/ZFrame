using ZFrame.MonoBase;

public abstract class MonoState : SingleMono, IGameState
{
	private void Start()
	{
		OnEnter();
	}

	private void Update()
	{
		OnStay();
	}

	private void OnDestroy()
	{
		OnExit();
	}

	public void ChangeState<T>() where T : MonoState
	{
		gameObject.AddComponent<T>();
	}

	#region Implementation of IGameState

	public abstract void OnEnter();

	public abstract void OnStay();

	public abstract void OnExit();

	#endregion
}