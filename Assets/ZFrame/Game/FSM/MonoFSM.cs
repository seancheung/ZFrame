using UnityEngine;

public abstract class MonoFSM : MonoBehaviour
{
	public abstract void OnEnter();
	public abstract void OnUpdate();
	public abstract void OnFixedUpdate();
	public abstract void OnExit();

	private void Start()
	{
		OnEnter();
	}

	private void Update()
	{
		OnUpdate();
	}

	private void FixedUpdate()
	{
		OnFixedUpdate();
	}

	private void OnDestroy()
	{
		OnExit();
	}
}