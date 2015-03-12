using System;

namespace ZFrame.Base.MonoBase
{
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
}