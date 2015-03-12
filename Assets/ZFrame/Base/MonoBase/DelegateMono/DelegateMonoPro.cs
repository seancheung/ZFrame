using System;
using UnityEngine;

namespace ZFrame.Base.MonoBase
{
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
}