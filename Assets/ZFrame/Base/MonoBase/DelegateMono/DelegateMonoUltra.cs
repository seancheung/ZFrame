using System;

namespace ZFrame.Base.MonoBase
{
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
}