using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class CardLoader : MonoBehaviour
{
	public UIGrid grid;

	private void Start()
	{
		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		foreach (CardData card in data.cards)
		{
			GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("CardListItem");
			CardListItem item = NGUITools.AddChild(grid.gameObject, cardobj).GetComponent<CardListItem>();
			item.cardName.text = card.name;
			item.cardType.text = card.type;
			item.cardCost.text = card.manaCost;
		}

		grid.Reposition();
	}
}