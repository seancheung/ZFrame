using UnityEngine;

public abstract class CardEntity : MonoBehaviour
{
	public int ID
	{
		get { return gameObject.GetInstanceID(); }
	}

	public Player owner;
	public Player controller;

	public CardInfo data;

	protected virtual void Start()
	{

	}
	
}