using System;
using UnityEngine;

public class CardListItem : MonoBehaviour
{
	public UILabel cardName;
	public UILabel cardType;
	public UILabel cardCost;
	public Expander expander;
	private CardData _data;

	public CardData Data
	{
		get { return _data; }
		set
		{
			_data = value;
			if (Data != null)
				Refresh();
		}
	}

	private void Refresh()
	{
		cardName.text = Data.name;
		cardType.text = CardRenderTool.ParseType(Data.types);
		cardCost.text = Data.manaCost;

		expander.name.text = Data.name;
		expander.cost.text = cardCost.text;
		expander.type.text = Data.type;
		expander.rarity.text = Data.rarity;
		expander.text.text = Data.text;
	}

	public void ShowCard()
	{
		SendMessageUpwards("ShowCardObj", Data);
	}

	[Serializable]
	public class Expander
	{
		public UILabel name;
		public UILabel cost;
		public UILabel type;
		public UILabel rarity;
		public UILabel text;
	}
}