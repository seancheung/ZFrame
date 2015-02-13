using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZFrame.IO;

public class DeckArea : MonoBehaviour
{
	public float zoomRatio = 1.5f;
	public GridLayoutGroup grid;
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
				item = ResourceEngine.Instance.LoadAndInstantiate("ListItem").GetComponent<ListItem>();
				item.transform.SetParent(grid.transform);
				item.Data = card.data;
				item.transform.localScale *= zoomRatio;
				_items.Add(item);
			}
		}
	}
}