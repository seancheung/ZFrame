using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class CardLoader : MonoBehaviour
{
	public UIGrid grid;
	public UIProgressBar progressBar;
	public UIPanel dockPanel;
	public UICamera cam;

	private const string MultiType = "[M]";
	private const string Creature = "[C]";
	private const string Artifact = "[A]";
	private const string Enchantment = "[E]";
	private const string Instant = "[I]";
	private const string Sorcery = "[S]";
	private const string PlanesWalker = "[P]";
	private const string Land = "[L]";

	private static readonly Dictionary<string, string> Types = new Dictionary<string, string>
	{
		{"creature", Creature},
		{"artifact", Artifact},
		{"enchantment", Enchantment},
		{"instant", Instant},
		{"sorcery", Sorcery},
		{"planeswalker", PlanesWalker},
		{"land", Land},
	};

	public void Load()
	{
		StartCoroutine(LoadCards());
	}

	private IEnumerator LoadCards()
	{
		grid.GetChildList().ForEach(t => Destroy(t.gameObject));

		progressBar.gameObject.SetActive(true);
		progressBar.value = 0;
		grid.gameObject.SetActive(false);
		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("CardListItem");
		for (int i = 0; i < data.cards.Length; i++)
		{
			CardData card = data.cards[i];
			CardListItem item = NGUITools.AddChild(grid.gameObject, cardobj).GetComponent<CardListItem>();
			item.Data = card;
			yield return item;

			progressBar.value = (float)i / data.cards.Length;
		}

		grid.gameObject.SetActive(true);
		grid.Reposition();
		progressBar.gameObject.SetActive(false);
	}

	private void ShowCardObj(CardData data)
	{
		GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("CardObj");
		var cardgo = NGUITools.AddChild(dockPanel.gameObject, cardobj).GetComponent<CardEntity>();
		cardgo.data = data;
		UIEventListener.Get(cardgo.transform.GetChild(0).gameObject).onDoubleClick += go =>
		{
			grid.gameObject.SetActive(true);
			Destroy(cardgo.gameObject);
			cam.GetComponent<TweenPosition>().PlayReverse();

		};
		cam.GetComponent<TweenPosition>().PlayForward();
		grid.gameObject.SetActive(false);
	}

	public static string ParseType(string[] types)
	{
		IEnumerable<string> res = types.Select(t => Types[t.ToLower().Trim()]);
		return res.Aggregate((a, b) => a + b);
	}
}