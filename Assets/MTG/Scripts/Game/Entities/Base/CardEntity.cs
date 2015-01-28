using UnityEngine;

public abstract class CardEntity : MonoBehaviour
{
	public int ID
	{
		get { return gameObject.GetInstanceID(); }
	}

	public Player owner;
	public Player controller;

	public CardData data;

	protected virtual void Start()
	{

	}
	
}