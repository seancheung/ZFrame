using System;
using UnityEngine;

namespace ZFrame.MonoBase
{
	public sealed class DelegateMonoFull : DelegateMonoUltra
	{
		/// <summary>
		/// <see cref="OnBecameVisible"/> is called when the renderer became visible by any camera
		/// </summary>
		public event Action OnBecameVisibleHandler;

		/// <summary>
		/// <see cref="OnBecameInvisible"/> is called when the renderer is no longer visible by any camera
		/// </summary>
		public event Action OnBecameInvisibleHandler;

		/// <summary>
		/// <see cref="OnGUI"/> is called for rendering and handling GUI events
		/// </summary>
		public event Action OnGUIHandler;

		/// <summary>
		/// <see cref="OnWillRenderObject"/> is called once for each camera if the object is visible
		/// </summary>
		public event Action OnWillRenderObjectHandler;

		private void OnBecameVisible()
		{
			if (OnBecameVisibleHandler != null) OnBecameVisibleHandler.Invoke();
		}

		private void OnBecameInvisible()
		{
			if (OnBecameInvisibleHandler != null) OnBecameInvisibleHandler.Invoke();
		}

		private void OnGUI()
		{
			if (OnGUIHandler != null) OnGUIHandler.Invoke();
		}

		private void OnWillRenderObject()
		{
			if (OnWillRenderObjectHandler != null) OnWillRenderObjectHandler.Invoke();
		}
	}

	public class DelegateMonoUltra : DelegateMonoPro
	{
		/// <summary>
		/// <see cref="OnMouseDown"/> is called when the user has pressed the mouse button while over the GUIElement or Collider
		/// </summary>
		public event Action OnMouseDownHandler;

		/// <summary>
		/// <see cref="OnMouseUp"/> is called when the user has released the mouse button
		/// </summary>
		public event Action OnMouseUpHandler;

		/// <summary>
		/// <see cref="OnMouseUpAsButton"/> is only called when the mouse is released over the same GUIElement or Collider as it was pressed
		/// </summary>
		public event Action OnMouseUpAsButtonHandler;

		/// <summary>
		/// <see cref="OnMouseOver"/> is called every frame while the mouse is over the GUIElement or Collider
		/// </summary>
		public event Action OnMouseOverHandler;

		/// <summary>
		/// <see cref="OnMouseExit"/> is called when the mouse is not any longer over the GUIElement or Collider
		/// </summary>
		public event Action OnMouseExitHandler;

		/// <summary>
		/// <see cref="OnMouseEnter"/> is called when the mouse entered the GUIElement or Collider
		/// </summary>
		public event Action OnMouseEnterHandler;

		/// <summary>
		/// <see cref="OnMouseDrag"/> is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
		/// </summary>
		public event Action OnMouseDragHandler;

		protected void OnMouseDown()
		{
			if (OnMouseDownHandler != null) OnMouseDownHandler.Invoke();
		}

		protected void OnMouseUp()
		{
			if (OnMouseUpHandler != null) OnMouseUpHandler.Invoke();
		}

		protected void OnMouseUpAsButton()
		{
			if (OnMouseUpAsButtonHandler != null) OnMouseUpAsButtonHandler.Invoke();
		}

		protected void OnMouseOver()
		{
			if (OnMouseOverHandler != null) OnMouseOverHandler.Invoke();
		}

		protected void OnMouseExit()
		{
			if (OnMouseExitHandler != null) OnMouseExitHandler.Invoke();
		}

		protected void OnMouseEnter()
		{
			if (OnMouseEnterHandler != null) OnMouseEnterHandler.Invoke();
		}

		protected void OnMouseDrag()
		{
			if (OnMouseDragHandler != null) OnMouseDragHandler.Invoke();
		}
	}

	public class DelegateMonoPro : DelegateMonoEx
	{
		/// <summary>
		/// <see cref="OnCollisionEnter"/> is called when this collider/rigidbody has begun touching another rigidbody/collider
		/// </summary>
		public event Action<Collision> OnCollisionEnterHandler;

		/// <summary>
		/// <see cref="OnCollisionExit"/> is called when this collider/rigidbody has stopped touching another rigidbody/collider
		/// </summary>
		public event Action<Collision> OnCollisionExitHandler;

		/// <summary>
		/// <see cref="OnCollisionStay"/> is called once per frame for every collider/rigidbody that is touching rigidbody/collider
		/// </summary>
		public event Action<Collision> OnCollisionStayHandler;

		/// <summary>
		/// <see cref="OnTriggerEnter"/> is called when the Collider other enters the trigger
		/// </summary>
		public event Action<Collider> OnTriggerEnterHandler;

		/// <summary>
		/// <see cref="OnTriggerExit"/> is called when the Collider other has stopped touching the trigger
		/// </summary>
		public event Action<Collider> OnTriggerExitHandler;

		/// <summary>
		/// <see cref="OnTriggerStay"/> is called once per frame for every Collider other that is touching the trigger
		/// </summary>
		public event Action<Collider> OnTriggerStayHandler;


		protected void OnCollisionEnter(Collision collision)
		{
			if (OnCollisionEnterHandler != null) OnCollisionEnterHandler.Invoke(collision);
		}

		protected void OnCollisionExit(Collision collisionInfo)
		{
			if (OnCollisionExitHandler != null) OnCollisionExitHandler.Invoke(collisionInfo);
		}

		protected void OnCollisionStay(Collision collisionInfo)
		{
			if (OnCollisionStayHandler != null) OnCollisionStayHandler.Invoke(collisionInfo);
		}

		protected void OnTriggerEnter(Collider other)
		{
			if (OnTriggerEnterHandler != null) OnTriggerEnterHandler.Invoke(other);
		}

		protected void OnTriggerExit(Collider other)
		{
			if (OnTriggerExitHandler != null) OnTriggerExitHandler.Invoke(other);
		}

		protected void OnTriggerStay(Collider other)
		{
			if (OnTriggerStayHandler != null) OnTriggerStayHandler.Invoke(other);
		}
	}

	public class DelegateMonoEx : DelegateMonoLite
	{
		/// <summary>
		/// <see cref="LateUpdate"/> is called every frame, if the Behaviour is enabled
		/// </summary>
		public event Action LateUpdateHandler;

		/// <summary>
		/// <see cref="FixedUpdate"/> is called every fixed framerate frame, if the MonoBehaviour is enabled
		/// </summary>
		public event Action FixedUpdateHandler;

		protected void LateUpdate()
		{
			if (LateUpdateHandler != null) LateUpdateHandler.Invoke();
		}

		protected void FixedUpdate()
		{
			if (FixedUpdateHandler != null) FixedUpdateHandler.Invoke();
		}
	}

	public class DelegateMonoLite : DelegateMonoMini
	{
		/// <summary>
		/// <see cref="OnDestroy"/> is called when the MonoBehaviour will be destroyed
		/// </summary>
		public event Action OnDestroyHandler;

		/// <summary>
		/// <see cref="OnDisable"/> is called when the behaviour becomes disabled () or inactive
		/// </summary>
		public event Action OnDisableHandler;

		/// <summary>
		/// <see cref="OnEnable"/> is called when the object becomes enabled and active
		/// </summary>
		public event Action OnEnableHandler;

		/// <summary>
		/// <see cref="OnLevelWasLoaded"/> is called after a new level was loaded
		/// </summary>
		public event Action<int> OnLevelWasLoadedHandler;

		/// <summary>
		/// <see cref="OnApplicationQuit"/> sent to all game objects before the application is quit
		/// </summary>
		public event Action OnApplicationQuitHandler;

		/// <summary>
		/// <see cref="OnApplicationFocus"/> sent to all game objects when the player gets or loses focus
		/// </summary>
		public event Action<bool> OnApplicationFocusHandler;

		/// <summary>
		/// <see cref="OnApplicationPause"/> sent to all game objects when the player pauses
		/// </summary>
		public event Action<bool> OnApplicationPauseHandler;

		protected void OnDestroy()
		{
			if (OnDestroyHandler != null) OnDestroyHandler.Invoke();
		}

		protected void OnDisable()
		{
			if (OnDisableHandler != null) OnDisableHandler.Invoke();
		}

		protected void OnEnable()
		{
			if (OnEnableHandler != null) OnEnableHandler.Invoke();
		}

		protected void OnLevelWasLoaded(int level)
		{
			if (OnLevelWasLoadedHandler != null) OnLevelWasLoadedHandler.Invoke(level);
		}

		protected void OnApplicationQuit()
		{
			if (OnApplicationQuitHandler != null) OnApplicationQuitHandler.Invoke();
		}

		protected void OnApplicationFocus(bool focusStatus)
		{
			if (OnApplicationFocusHandler != null) OnApplicationFocusHandler.Invoke(focusStatus);
		}

		protected void OnApplicationPause(bool pauseStatus)
		{
			if (OnApplicationPauseHandler != null) OnApplicationPauseHandler.Invoke(pauseStatus);
		}
	}

	/// <summary>
	/// Basic mono methods: Start(),Update()
	/// </summary>
	public class DelegateMonoMini : DelegateMono
	{
		/// <summary>
		/// <see cref="Start"/> is called on the frame when a script is enabled just before any of the Update methods is called the first time
		/// </summary>
		public event Action StartHandler;

		/// <summary>
		/// <see cref="Update"/> is called every frame, if the MonoBehaviour is enabled
		/// </summary>
		public event Action UpdateHandler;

		protected void Start()
		{
			if (StartHandler != null) StartHandler.Invoke();
		}

		protected void Update()
		{
			if (UpdateHandler != null) UpdateHandler.Invoke();
		}
	}

	public abstract class DelegateMono : MonoBehaviour
	{
	}
}