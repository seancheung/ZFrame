using System;

namespace ZFrame.Base.MonoBase
{
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
}