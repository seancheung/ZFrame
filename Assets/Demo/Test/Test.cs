using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class Test : MonoBehaviour
{
	//public UIGrid grid;
	public TextAsset text;
	private CSVRecord csv;

	private void Start()
	{
		//SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);
		//foreach (CardData card in data.cards)
		//{
		//	GameObject cardobj = ResourceEngine.Instance.Load<GameObject>("CardObj");
		//	var cardgo = NGUITools.AddChild(grid.gameObject, cardobj).GetComponent<CardEntity>();
		//	cardgo.data = card;
		//	cardgo.GetComponent<CardRenderer>().frameSprite.gameObject.AddComponent<UICenterOnClick>().gameObject.AddComponent<UIDragScrollView>();
		//}

		//grid.Reposition();
		var reader = new CSVReader();
		csv = reader.Read(text.text);
		Debug.Log(csv.IsMatrix);
	}

	void OnGUI()
	{
		if (csv != null)
		{
			for (int i = 0; i < csv.Count; i++)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(csv[i].Count.ToString());
					for (int j = 0; j < csv[i].Count; j++)
					{
						GUILayout.Button(csv[i][j],GUILayout.MinWidth(25));
					}
				}
				GUILayout.EndHorizontal();
			}
		}
	}
}