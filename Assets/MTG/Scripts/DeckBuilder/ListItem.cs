using UnityEngine;

public class ListItem : MonoBehaviour
{
	public UILabel header;
	public UILabel cardName;
	public UILabel quantity;
	private int _quantity;
	public CardData data;

	public int Quantity
	{
		get { return _quantity; }
		set
		{
			_quantity = value;
			quantity.text = Quantity.ToString();
		}
	}

	public CardData Data
	{
		get { return data; }
		set
		{
			data = value;
			if (Data != null)
			{
				header.text = CardRenderTool.ParseType(Data.types);
				cardName.text = Data.name;
				Quantity = 1;
			}
		}
	}
}