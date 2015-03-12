using System;

namespace ZFrame.Base.MonoBase
{
    public sealed class DelegateMonoUltimate : DelegateMonoUltra
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
}