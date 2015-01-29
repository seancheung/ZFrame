using UnityEngine;

public abstract class SingleMono : MonoBehaviour
{
	protected virtual void Start()
	{
		foreach (SingleMono component in GetComponents<SingleMono>())
		{
			if (component != this)
				Destroy(component);
		}
	}
}