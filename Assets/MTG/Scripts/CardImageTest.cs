using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class CardImageTest : MonoBehaviour
{
	public UIGrid grid;

	private void Start()
	{
		StartCoroutine(LoadCards());
	}

	private IEnumerator LoadCards()
	{
		grid.GetChildList().ForEach(t => Destroy(t.gameObject));
		grid.gameObject.SetActive(false);
		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("Card");
		for (int i = 0; i < data.cards.Length; i++)
		{
			CardData card = data.cards[i];
			CardRenderer item = NGUITools.AddChild(grid.gameObject, cardobj).GetComponent<CardRenderer>();
			item.data = card;
			yield return item;
		}

		grid.gameObject.SetActive(true);
		grid.Reposition();
	}
}