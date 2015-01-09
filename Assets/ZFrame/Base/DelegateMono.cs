using System;
using UnityEngine;

public sealed class DelegateMono : MonoBehaviour
{
	/// <summary>
	/// <see cref="Awake"/> is called when the script instance is being loaded
	/// </summary>
	public event Action AwakeEvent;

	/// <summary>
	/// <see cref="Start"/> is called on the frame when a script is enabled just before any of the Update methods is called the first time
	/// </summary>
	public event Action StartEvent;

	/// <summary>
	/// <see cref="FixedUpdate"/> is called every fixed framerate frame, if the MonoBehaviour is enabled
	/// </summary>
	public event Action FixedUpdateEvent;

	/// <summary>
	/// <see cref="LateUpdate"/> is called every frame, if the Behaviour is enabled
	/// </summary>
	public event Action LateUpdateEvent;

	/// <summary>
	/// <see cref="Update"/> is called every frame, if the MonoBehaviour is enabled
	/// </summary>
	public event Action UpdateEvent;

	/// <summary>
	/// <see cref="OnApplicationFocus"/> sent to all game objects when the player gets or loses focus
	/// </summary>
	public event Action<bool> OnApplicationFocusEvent;

	/// <summary>
	/// <see cref="OnApplicationPause"/> sent to all game objects when the player pauses
	/// </summary>
	public event Action<bool> OnApplicationPauseEvent;

	/// <summary>
	/// <see cref="OnApplicationQuit"/> sent to all game objects before the application is quit
	/// </summary>
	public event Action OnApplicationQuitEvent;

	/// <summary>
	/// <see cref="OnBecameVisible"/> is called when the renderer became visible by any camera
	/// </summary>
	public event Action onBecameVisible;

	/// <summary>
	/// <see cref="OnBecameInvisible"/> is called when the renderer is no longer visible by any camera
	/// </summary>
	public event Action onBecameInvisible;

	/// <summary>
	/// <see cref="OnCollisionEnter"/> is called when this collider/rigidbody has begun touching another rigidbody/collider
	/// </summary>
	public event Action<Collision> onCollisionEnter;

	/// <summary>
	/// <see cref="OnCollisionExit"/> is called when this collider/rigidbody has stopped touching another rigidbody/collider
	/// </summary>
	public event Action<Collision> onCollisionExit;

	/// <summary>
	/// <see cref="OnCollisionStay"/> is called once per frame for every collider/rigidbody that is touching rigidbody/collider
	/// </summary>
	public event Action<Collision> onCollisionStay;

	/// <summary>
	/// <see cref="OnDestroy"/> is called when the MonoBehaviour will be destroyed
	/// </summary>
	public event Action onDestroy;

	/// <summary>
	/// <see cref="OnDisable"/> is called when the behaviour becomes disabled () or inactive
	/// </summary>
	public event Action onDisable;

	/// <summary>
	/// <see cref="OnEnable"/> is called when the object becomes enabled and active
	/// </summary>
	public event Action onEnable;

	/// <summary>
	/// <see cref="OnGUI"/> is called for rendering and handling GUI events
	/// </summary>
	public event Action onGUI;

	/// <summary>
	/// <see cref="OnLevelWasLoaded"/> is called after a new level was loaded
	/// </summary>
	public event Action<int> onLevelWasLoaded;

	/// <summary>
	/// <see cref="OnMouseDown"/> is called when the user has pressed the mouse button while over the GUIElement or Collider
	/// </summary>
	public event Action onMouseDown;

	/// <summary>
	/// <see cref="OnMouseUp"/> is called when the user has released the mouse button
	/// </summary>
	public event Action onMouseUp;

	/// <summary>
	/// <see cref="OnMouseUpAsButton"/> is only called when the mouse is released over the same GUIElement or Collider as it was pressed
	/// </summary>
	public event Action onMouseUpAsButton;

	/// <summary>
	/// <see cref="OnMouseOver"/> is called every frame while the mouse is over the GUIElement or Collider
	/// </summary>
	public event Action onMouseOver;

	/// <summary>
	/// <see cref="OnMouseExit"/> is called when the mouse is not any longer over the GUIElement or Collider
	/// </summary>
	public event Action onMouseExit;

	/// <summary>
	/// <see cref="OnMouseEnter"/> is called when the mouse entered the GUIElement or Collider
	/// </summary>
	public event Action onMouseEnter;

	/// <summary>
	/// <see cref="OnMouseDrag"/> is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
	/// </summary>
	public event Action OnMouseDragEvent;

	/// <summary>
	/// <see cref="OnTriggerEnter"/> is called when the Collider other enters the trigger
	/// </summary>
	public event Action<Collider> OnTriggerEnterEvent;

	/// <summary>
	/// <see cref="OnTriggerExit"/> is called when the Collider other has stopped touching the trigger
	/// </summary>
	public event Action<Collider> OnTriggerExitEvent;

	/// <summary>
	/// <see cref="OnTriggerStay"/> is called once per frame for every Collider other that is touching the trigger
	/// </summary>
	public event Action<Collider> OnTriggerStayEvent;

	/// <summary>
	/// <see cref="OnWillRenderObject"/> is called once for each camera if the object is visible
	/// </summary>
	public event Action OnWillRenderObjectEvent;


	private void Awake()
	{
		if (AwakeEvent != null) AwakeEvent.Invoke();
	}

	private void Start()
	{
		if (StartEvent != null) StartEvent.Invoke();
	}

	private void Update()
	{
		if (UpdateEvent != null) UpdateEvent.Invoke();
	}

	private void FixedUpdate()
	{
		if (FixedUpdateEvent != null) FixedUpdateEvent.Invoke();
	}

	private void LateUpdate()
	{
		if (LateUpdateEvent != null) LateUpdateEvent.Invoke();
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (OnApplicationFocusEvent != null) OnApplicationFocusEvent.Invoke(focusStatus);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (OnApplicationPauseEvent != null) OnApplicationPauseEvent.Invoke(pauseStatus);
	}

	private void OnApplicationQuit()
	{
		if (OnApplicationQuitEvent != null) OnApplicationQuitEvent.Invoke();
	}

	private void OnBecameVisible()
	{
		if (onBecameVisible != null) onBecameVisible.Invoke();
	}

	private void OnBecameInvisible()
	{
		if (onBecameInvisible != null) onBecameInvisible.Invoke();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (onCollisionEnter != null) onCollisionEnter.Invoke(collision);
	}

	private void OnCollisionExit(Collision collisionInfo)
	{
		if (onCollisionExit != null) onCollisionExit.Invoke(collisionInfo);
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		if (onCollisionStay != null) onCollisionStay.Invoke(collisionInfo);
	}

	private void OnDestroy()
	{
		if (onDestroy != null) onDestroy.Invoke();
	}

	private void OnDisable()
	{
		if (onDisable != null) onDisable.Invoke();
	}

	private void OnEnable()
	{
		if (onEnable != null) onEnable.Invoke();
	}

	private void OnGUI()
	{
		if (onGUI != null) onGUI.Invoke();
	}

	private void OnLevelWasLoaded(int level)
	{
		if (onLevelWasLoaded != null) onLevelWasLoaded.Invoke(level);
	}

	private void OnMouseDown()
	{
		if (onMouseDown != null) onMouseDown.Invoke();
	}

	private void OnMouseUp()
	{
		if (onMouseUp != null) onMouseUp.Invoke();
	}

	private void OnMouseUpAsButton()
	{
		if (onMouseUpAsButton != null) onMouseUpAsButton.Invoke();
	}

	private void OnMouseOver()
	{
		if (onMouseOver != null) onMouseOver.Invoke();
	}

	private void OnMouseExit()
	{
		if (onMouseExit != null) onMouseExit.Invoke();
	}

	private void OnMouseEnter()
	{
		if (onMouseEnter != null) onMouseEnter.Invoke();
	}

	private void OnMouseDrag()
	{
		if (OnMouseDragEvent != null) OnMouseDragEvent.Invoke();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (OnTriggerEnterEvent != null) OnTriggerEnterEvent.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		if (OnTriggerExitEvent != null) OnTriggerExitEvent.Invoke(other);
	}

	private void OnTriggerStay(Collider other)
	{
		if (OnTriggerStayEvent != null) OnTriggerStayEvent.Invoke(other);
	}

	private void OnWillRenderObject()
	{
		if (OnWillRenderObjectEvent != null) OnWillRenderObjectEvent.Invoke();
	}
}