using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class DelegateMono : MonoBehaviour
{
	public Dictionary<string, List<Delegate>> delegates = new Dictionary<string, List<Delegate>>();

	private void AddDelegate(string key, Delegate value)
	{
		if (!delegates.ContainsKey(key))
			delegates.Add(key, new List<Delegate>());
		delegates[key].Add(value);
	}

	private void RemoveDelegate(string key, Delegate value)
	{
		if (delegates.ContainsKey(key))
			delegates[key].RemoveAll(m => m == value);
	}

	private void InvokeDelegates(string key, params object[] args)
	{
		if (delegates.ContainsKey(key))
			delegates[key].ForEach(m => m.DynamicInvoke(args));
	}

	///// <summary>
	///// <see cref="Awake"/> is called when the script instance is being loaded
	///// </summary>
	//public event Action AwakeEvent
	//{
	//	add { AddDelegate("Awake", new ZMethod(value.Target, value)); }
	//	remove { RemoveDelegate("Awake", value); }
	//}

	/// <summary>
	/// <see cref="Start"/> is called on the frame when a script is enabled just before any of the Update methods is called the first time
	/// </summary>
	public event Action StartEvent
	{
		add { AddDelegate("Start", value); }
		remove { RemoveDelegate("Start", value); }
	}

	/// <summary>
	/// <see cref="FixedUpdate"/> is called every fixed framerate frame, if the MonoBehaviour is enabled
	/// </summary>
	public event Action FixedUpdateEvent
	{
		add { RemoveDelegate("FixedUpdate", value); }
		remove { RemoveDelegate("FixedUpdate", value); }
	}

	/// <summary>
	/// <see cref="LateUpdate"/> is called every frame, if the Behaviour is enabled
	/// </summary>
	public event Action LateUpdateEvent
	{
		add { RemoveDelegate("LateUpdate", value); }
		remove { RemoveDelegate("LateUpdate", value); }
	}

	/// <summary>
	/// <see cref="Update"/> is called every frame, if the MonoBehaviour is enabled
	/// </summary>
	public event Action UpdateEvent
	{
		add { RemoveDelegate("Update", value); }
		remove { RemoveDelegate("Update", value); }
	}

	/// <summary>
	/// <see cref="OnApplicationFocus"/> sent to all game objects when the player gets or loses focus
	/// </summary>
	public event Action<bool> OnApplicationFocusEvent
	{
		add { RemoveDelegate("OnApplicationFocus", value); }
		remove { RemoveDelegate("OnApplicationFocus", value); }
	}

	/// <summary>
	/// <see cref="OnApplicationPause"/> sent to all game objects when the player pauses
	/// </summary>
	public event Action<bool> OnApplicationPauseEvent
	{
		add { RemoveDelegate("OnApplicationPause", value); }
		remove { RemoveDelegate("OnApplicationPause", value); }
	}

	/// <summary>
	/// <see cref="OnApplicationQuit"/> sent to all game objects before the application is quit
	/// </summary>
	public event Action OnApplicationQuitEvent
	{
		add { RemoveDelegate("OnApplicationQuit", value); }
		remove { RemoveDelegate("OnApplicationQuit", value); }
	}

	/// <summary>
	/// <see cref="OnBecameVisible"/> is called when the renderer became visible by any camera
	/// </summary>
	public event Action OnBecameVisibleEvent
	{
		add { RemoveDelegate("OnBecameVisible", value); }
		remove { RemoveDelegate("OnBecameVisible", value); }
	}

	/// <summary>
	/// <see cref="OnBecameInvisible"/> is called when the renderer is no longer visible by any camera
	/// </summary>
	public event Action OnBecameInvisibleEvent
	{
		add { RemoveDelegate("OnBecameInvisible", value); }
		remove { RemoveDelegate("OnBecameInvisible", value); }
	}

	/// <summary>
	/// <see cref="OnCollisionEnter"/> is called when this collider/rigidbody has begun touching another rigidbody/collider
	/// </summary>
	public event Action<Collision> OnCollisionEnterEvent
	{
		add { RemoveDelegate("OnCollisionEnter", value); }
		remove { RemoveDelegate("OnCollisionEnter", value); }
	}

	/// <summary>
	/// <see cref="OnCollisionExit"/> is called when this collider/rigidbody has stopped touching another rigidbody/collider
	/// </summary>
	public event Action<Collision> OnCollisionExitEvent
	{
		add { RemoveDelegate("OnCollisionExit", value); }
		remove { RemoveDelegate("OnCollisionExit", value); }
	}

	/// <summary>
	/// <see cref="OnCollisionStay"/> is called once per frame for every collider/rigidbody that is touching rigidbody/collider
	/// </summary>
	public event Action<Collision> OnCollisionStayEvent
	{
		add { RemoveDelegate("OnCollisionStay", value); }
		remove { RemoveDelegate("OnCollisionStay", value); }
	}

	/// <summary>
	/// <see cref="OnDestroy"/> is called when the MonoBehaviour will be destroyed
	/// </summary>
	public event Action OnDestroyEvent
	{
		add { RemoveDelegate("OnDestroy", value); }
		remove { RemoveDelegate("OnDestroy", value); }
	}

	/// <summary>
	/// <see cref="OnDisable"/> is called when the behaviour becomes disabled () or inactive
	/// </summary>
	public event Action OnDisableEvent
	{
		add { RemoveDelegate("OnDisable", value); }
		remove { RemoveDelegate("OnDisable", value); }
	}

	/// <summary>
	/// <see cref="OnEnable"/> is called when the object becomes enabled and active
	/// </summary>
	public event Action OnEnableEvent
	{
		add { RemoveDelegate("OnEnable", value); }
		remove { RemoveDelegate("OnEnable", value); }
	}

	/// <summary>
	/// <see cref="OnGUI"/> is called for rendering and handling GUI events
	/// </summary>
	public event Action OnGUIEvent
	{
		add { RemoveDelegate("OnGUI", value); }
		remove { RemoveDelegate("OnGUI", value); }
	}

	/// <summary>
	/// <see cref="OnLevelWasLoaded"/> is called after a new level was loaded
	/// </summary>
	public event Action<int> OnLevelWasLoadedEvent
	{
		add { RemoveDelegate("OnLevelWasLoaded", value); }
		remove { RemoveDelegate("OnLevelWasLoaded", value); }
	}

	/// <summary>
	/// <see cref="OnMouseDown"/> is called when the user has pressed the mouse button while over the GUIElement or Collider
	/// </summary>
	public event Action OnMouseDownEvent
	{
		add { RemoveDelegate("OnMouseDown", value); }
		remove { RemoveDelegate("OnMouseDown", value); }
	}

	/// <summary>
	/// <see cref="OnMouseUp"/> is called when the user has released the mouse button
	/// </summary>
	public event Action OnMouseUpEvent
	{
		add { RemoveDelegate("OnMouseUp", value); }
		remove { RemoveDelegate("OnMouseUp", value); }
	}

	/// <summary>
	/// <see cref="OnMouseUpAsButton"/> is only called when the mouse is released over the same GUIElement or Collider as it was pressed
	/// </summary>
	public event Action OnMouseUpAsButtonEvent
	{
		add { RemoveDelegate("OnMouseUpAsButton", value); }
		remove { RemoveDelegate("OnMouseUpAsButton", value); }
	}

	/// <summary>
	/// <see cref="OnMouseOver"/> is called every frame while the mouse is over the GUIElement or Collider
	/// </summary>
	public event Action OnMouseOverEvent
	{
		add { RemoveDelegate("OnMouseOver", value); }
		remove { RemoveDelegate("OnMouseOver", value); }
	}

	/// <summary>
	/// <see cref="OnMouseExit"/> is called when the mouse is not any longer over the GUIElement or Collider
	/// </summary>
	public event Action OnMouseExitEvent
	{
		add { RemoveDelegate("OnMouseExit", value); }
		remove { RemoveDelegate("OnMouseExit", value); }
	}

	/// <summary>
	/// <see cref="OnMouseEnter"/> is called when the mouse entered the GUIElement or Collider
	/// </summary>
	public event Action OnMouseEnterEvent
	{
		add { RemoveDelegate("OnMouseEnter", value); }
		remove { RemoveDelegate("OnMouseEnter", value); }
	}

	/// <summary>
	/// <see cref="OnMouseDrag"/> is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
	/// </summary>
	public event Action OnMouseDragEvent
	{
		add { RemoveDelegate("OnMouseDrag", value); }
		remove { RemoveDelegate("OnMouseDrag", value); }
	}

	/// <summary>
	/// <see cref="OnTriggerEnter"/> is called when the Collider other enters the trigger
	/// </summary>
	public event Action<Collider> OnTriggerEnterEvent
	{
		add { RemoveDelegate("OnTriggerEnter", value); }
		remove { RemoveDelegate("OnTriggerEnter", value); }
	}

	/// <summary>
	/// <see cref="OnTriggerExit"/> is called when the Collider other has stopped touching the trigger
	/// </summary>
	public event Action<Collider> OnTriggerExitEvent
	{
		add { RemoveDelegate("OnTriggerExit", value); }
		remove { RemoveDelegate("OnTriggerExit", value); }
	}

	/// <summary>
	/// <see cref="OnTriggerStay"/> is called once per frame for every Collider other that is touching the trigger
	/// </summary>
	public event Action<Collider> OnTriggerStayEvent
	{
		add { RemoveDelegate("OnTriggerStay", value); }
		remove { RemoveDelegate("OnTriggerStay", value); }
	}

	/// <summary>
	/// <see cref="OnWillRenderObject"/> is called once for each camera if the object is visible
	/// </summary>
	public event Action OnWillRenderObjectEvent
	{
		add { RemoveDelegate("OnWillRenderObject", value); }
		remove { RemoveDelegate("OnWillRenderObject", value); }
	}


	//private void Awake()
	//{
	//	InvokeDelegates("Awake");
	//}

	private void Start()
	{
		InvokeDelegates("Start");
	}

	private void Update()
	{
		InvokeDelegates("Update");
	}

	private void FixedUpdate()
	{
		InvokeDelegates("FixedUpdate");
	}

	private void LateUpdate()
	{
		InvokeDelegates("LateUpdate");
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		InvokeDelegates("OnApplicationFocus");
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		InvokeDelegates("OnApplicationPause");
	}

	private void OnApplicationQuit()
	{
		InvokeDelegates("OnApplicationQuit");
	}

	private void OnBecameVisible()
	{
		InvokeDelegates("OnBecameVisible");
	}

	private void OnBecameInvisible()
	{
		InvokeDelegates("OnBecameInvisible");
	}

	private void OnCollisionEnter(Collision collision)
	{
		InvokeDelegates("OnCollisionEnter");
	}

	private void OnCollisionExit(Collision collisionInfo)
	{
		InvokeDelegates("OnCollisionExit");
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		InvokeDelegates("OnCollisionStay");
	}

	private void OnDestroy()
	{
		InvokeDelegates("OnDestroy");
	}

	private void OnDisable()
	{
		InvokeDelegates("OnDisable");
	}

	private void OnEnable()
	{
		InvokeDelegates("OnEnable");
	}

	private void OnGUI()
	{
		InvokeDelegates("OnGUI");
	}

	private void OnLevelWasLoaded(int level)
	{
		InvokeDelegates("OnLevelWasLoaded");
	}

	private void OnMouseDown()
	{
		InvokeDelegates("OnMouseDown");
	}

	private void OnMouseUp()
	{
		InvokeDelegates("OnMouseUp");
	}

	private void OnMouseUpAsButton()
	{
		InvokeDelegates("OnMouseUpAsButton");
	}

	private void OnMouseOver()
	{
		InvokeDelegates("OnMouseOver");
	}

	private void OnMouseExit()
	{
		InvokeDelegates("OnMouseExit");
	}

	private void OnMouseEnter()
	{
		InvokeDelegates("OnMouseEnter");
	}

	private void OnMouseDrag()
	{
		InvokeDelegates("OnMouseDrag");
	}

	private void OnTriggerEnter(Collider other)
	{
		InvokeDelegates("OnTriggerEnter");
	}

	private void OnTriggerExit(Collider other)
	{
		InvokeDelegates("OnTriggerExit");
	}

	private void OnTriggerStay(Collider other)
	{
		InvokeDelegates("OnTriggerStay");
	}

	private void OnWillRenderObject()
	{
		InvokeDelegates("OnWillRenderObject");
	}
}