using UnityEngine;

public class RotateComponent : MonoBehaviour
{
	public Vector3 speed;

	void Update()
	{
		transform.Rotate(speed*Time.deltaTime);
	}
}