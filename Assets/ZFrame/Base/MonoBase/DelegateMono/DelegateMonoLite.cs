using System;

namespace ZFrame.Base.MonoBase
{
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
}