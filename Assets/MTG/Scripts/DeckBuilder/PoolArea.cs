using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using ZFrame.IO;

public class PoolArea : MonoBehaviour
{
	public UITable tab;

	IEnumerator Start()
	{
		tab.GetChildList().ForEach(t => Destroy(t.gameObject));
		tab.gameObject.SetActive(false);

		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("Card");

		foreach (CardData card in data.cards)
		{
			var item = NGUITools.AddChild(tab.gameObject, cardobj).GetComponent<CardRenderer>();
			item.data = card;
			item.transform.localScale *= 0.5f;
			item.gameObject.AddComponent<UIDragDropItem>().cloneOnDrag = true;
			yield return item;
		}

		tab.gameObject.SetActive(true);
		tab.Reposition();
	}
}