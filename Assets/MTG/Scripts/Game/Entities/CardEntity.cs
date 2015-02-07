using UnityEngine;

public class CardEntity : MonoBehaviour
{
	public int ID
	{
		get { return gameObject.GetInstanceID(); }
	}

	public PlayerEntity owner;
	public PlayerEntity controller;

	public CardData data;
}