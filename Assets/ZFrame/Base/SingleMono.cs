using UnityEngine;

namespace ZFrame.MonoBase
{
	public abstract class SingleMono : MonoBehaviour
	{
		protected virtual void Awake()
		{
			foreach (SingleMono component in GetComponents<SingleMono>())
			{
				if (component != this)
					Destroy(component);
			}
		}
	}
}