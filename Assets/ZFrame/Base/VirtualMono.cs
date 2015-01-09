using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualMono
{
	private GameObject Mono { get; set; }
	private readonly Queue<string> _dispatcher;

	protected VirtualMono()
	{
		Mono = new GameObject(GetType().ToString(), typeof (VirtualMonoManager));
		_dispatcher = Mono.GetComponent<VirtualMonoManager>().dispatcher;
	}

	~VirtualMono()
	{
		if (_dispatcher != null)
		{
			_dispatcher.Enqueue(VirtualMonoManager.MsgDestroy);
		}
	}

	public void AddComponent<TComponent>() where TComponent : MonoBehaviour
	{
		Mono.AddComponent<TComponent>();
	}

	public TComponent GetComponent<TComponent>() where TComponent : MonoBehaviour
	{
		return Mono.GetComponent<TComponent>();
	}
}

public sealed class VirtualMonoManager : MonoBehaviour
{
	public const string MsgDestroy = "Destroy";

	public Action onAwake;
	public Action onStart;
	public Action onDestroy;
	public Queue<string> dispatcher = new Queue<string>();

	private void Awake()
	{
		if (onAwake != null) onAwake.Invoke();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (onStart != null) onStart.Invoke();
	}

	private void Update()
	{
		if (dispatcher.Count > 0)
		{
			string msg = dispatcher.Dequeue();
			switch (msg)
			{
				case MsgDestroy:
					Destroy(gameObject);
					break;
			}
		}
	}

	private void OnDestroy()
	{
		if (onDestroy != null) onDestroy.Invoke();
	}
}