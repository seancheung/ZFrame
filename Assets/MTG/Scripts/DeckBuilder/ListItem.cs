using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
	public Text header;
	public Text cardName;
	public Text quantity;
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