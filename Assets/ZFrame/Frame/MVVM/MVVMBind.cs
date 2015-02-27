using System;
using UnityEngine;

namespace ZFrame.Frame.MVVM
{
    public class MVVMBind : MonoBehaviour
    {
        private void Start()
        {
            foreach (MonoBehaviour item in GetComponents<MonoBehaviour>())
            {
                if (Attribute.IsDefined(item.GetType(), typeof (ViewModelAttribute)) ||
                    Attribute.IsDefined(item.GetType(), typeof (ViewAttribute)))
                {
                    MVVMEngine.Instance.Register(item);
                }
            }
        }
    }
}