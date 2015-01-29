using System;
using UnityEngine;

public class MonoExecute : MonoBehaviour
{
	public event Action<object> onStart;
	public event Action<object> onUpdate;
	public event Action<object> onComplete;
	public object onStartParameter;
	public object onUpdateParameter;
	public object onCompleteParameter;
	public float time;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (onStart != null) onStart.Invoke(onStartParameter);
	}

	private void Update()
	{
		if ((time -= Time.deltaTime) <= 0)
		{
			if (onComplete != null) onComplete.Invoke(onCompleteParameter);
			Destroy(gameObject);
		}
		else
		{
			if (onUpdate != null) onUpdate.Invoke(onUpdateParameter);
			gameObject.name = "MonoExecute_" + time;
		}
	}


	/// <summary>
	/// 计时委托(委托可以源于非Monobehaviour)
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <param name="time">计时时长(秒)</param>
	/// <param name="onStart">计时开始时要执行的委托</param>
	/// <param name="onStartParameter">计时开始委托参数</param>
	/// <param name="onUpdate">计时过程中执行的委托(每帧)</param>
	/// <param name="onUpdateParameter">计时过程中委托参数</param>
	/// <param name="onComplete">计时完成后执行的委托</param>
	/// <param name="onCompleteParameter">计时完成后委托参数</param>
	public static void Execute<T1, T2, T3>(float time, Action<T1> onStart, T1 onStartParameter, Action<T2> onUpdate,
		T2 onUpdateParameter, Action<T3> onComplete, T3 onCompleteParameter)
	{
		GameObject temp = new GameObject("MonoExecute_" + time);
		MonoExecute mono = temp.AddComponent<MonoExecute>();
		if (onStart != null) mono.onStart += obj => onStart((T1) obj);
		if (onUpdate != null) mono.onUpdate += obj => onUpdate((T2) obj);
		if (onComplete != null) mono.onComplete += obj => onComplete((T3) obj);
		mono.onStartParameter = onStartParameter;
		mono.onUpdateParameter = onUpdateParameter;
		mono.onCompleteParameter = onCompleteParameter;
		mono.time = time;
	}


	/// <summary>
	/// 计时委托(委托可以源于非Monobehaviour)
	/// </summary>
	/// <param name="time">计时时长(秒)</param>
	/// <param name="onStart">计时开始时要执行的委托</param>
	/// <param name="onUpdate">计时过程中执行的委托(每帧)</param>
	/// <param name="onComplete">计时完成后执行的委托</param>
	public static void Execute(float time, Action onStart, Action onUpdate, Action onComplete)
	{
		GameObject temp = new GameObject("MonoExecute_" + time);
		MonoExecute mono = temp.AddComponent<MonoExecute>();
		if (onStart != null) mono.onStart += obj => onStart();
		if (onUpdate != null) mono.onUpdate += obj => onUpdate();
		if (onComplete != null) mono.onComplete += obj => onComplete();
		mono.time = time;
	}

	/// <summary>
	/// 计时委托(委托可以源于非Monobehaviour)
	/// </summary>
	/// <param name="time">计时时长(秒)</param>
	/// /// <param name="type">回调执行时机类型</param>
	/// <param name="action">要执行的回调</param>
	public static void Execute(float time, MonoExecuteType type, Action action)
	{
		switch (type)
		{
			case MonoExecuteType.OnStart:
				Execute(time, action, null, null);
				break;
			case MonoExecuteType.OnUpdate:
				Execute(time, null, action, null);
				break;
			case MonoExecuteType.OnComplete:
				Execute(time, null, null, action);
				break;
			default:
				throw new ArgumentOutOfRangeException("type");
		}
	}

	/// <summary>
	/// 计时委托(委托可以源于非Monobehaviour)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="time">计时时长(秒)</param>
	/// /// <param name="type">回调执行时机类型</param>
	/// <param name="action">要执行的回调</param>
	/// <param name="parameter">回调参数</param>
	public static void Execute<T>(float time, MonoExecuteType type, Action<T> action, T parameter)
	{
		switch (type)
		{
			case MonoExecuteType.OnStart:
				Execute(time, action, parameter, null, parameter, null, parameter);
				break;
			case MonoExecuteType.OnUpdate:
				Execute(time, null, parameter, action, parameter, null, parameter);
				break;
			case MonoExecuteType.OnComplete:
				Execute(time, null, parameter, null, parameter, action, parameter);
				break;
			default:
				throw new ArgumentOutOfRangeException("type");
		}
	}

	/// <summary>
	/// 停止场景里的所有MonoExecute
	/// </summary>
	public static void StopAllExecute()
	{
		foreach (MonoExecute execute in FindObjectsOfType<MonoExecute>())
		{
			Destroy(execute.gameObject);
		}
	}
}

/// <summary>
/// 回调执行时机类型
/// </summary>
public enum MonoExecuteType
{
	OnStart,
	OnUpdate,
	OnComplete
}