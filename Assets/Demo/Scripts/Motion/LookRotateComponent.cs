using UnityEngine;

public class LookRotateComponent : MonoBehaviour
{
	public Transform target;
	public float speed;
	public Vector3 axis;


	void Update()
	{
		transform.RotateAround(target.position, axis, speed*Time.deltaTime);
		transform.LookAt(target);
	}
}