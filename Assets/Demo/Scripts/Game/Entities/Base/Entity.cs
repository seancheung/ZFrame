using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public int ID
	{
		get { return gameObject.GetInstanceID(); }
	}

	public new string name;
	public Player owner;
	public Player controller;
}