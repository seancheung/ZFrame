using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZFrame.IO;

public class DeckArea : MonoBehaviour
{
	public UIGrid grid;
	private readonly List<ListItem> _items = new List<ListItem>();

	private void OnDrop(GameObject go)
	{
		CardRenderer card = go.GetComponent<CardRenderer>();
		if (card)
		{
			ListItem item = _items.FirstOrDefault(i => i.Data.multiverseid == card.data.multiverseid);
			if (item)
				item.Quantity ++;
			else
			{
				GameObject listitem = ResourceEngine.Instance.Load<GameObject>("ListItem");
				item = NGUITools.AddChild(grid.gameObject, listitem).GetComponent<ListItem>();
				item.Data = card.data;
				_items.Add(item);
				grid.Reposition();
			}
		}
	}
}