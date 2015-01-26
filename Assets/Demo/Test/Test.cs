using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class Test : MonoBehaviour
{
	public UIGrid grid;

	private void Start()
	{
		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		foreach (CardData card in data.cards)
		{
			GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("CardObj");
			var cardgo = NGUITools.AddChild(grid.gameObject, cardobj).GetComponent<CardEntity>();
			cardgo.data = card;
			cardgo.GetComponent<CardRenderer>().frameSprite.gameObject.AddComponent<UICenterOnClick>().gameObject.AddComponent<UIDragScrollView>();
		}

		grid.Reposition();
	}
}