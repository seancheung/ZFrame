using UnityEngine;

[RequireComponent(typeof (CardEntity))]
public class CardRenderer : MonoBehaviour
{
	public UISprite frameSprite;
	public UILabel nameLabel;
	public UILabel costLabel;
	public UILabel typeLabel;
	public UISprite raritySprite;
	public UILabel textLabel;

	private void Start()
	{
		Refresh();
	}

	private void Refresh()
	{
		CardData data = GetComponent<CardEntity>().data;
		nameLabel.text = data.name;
		costLabel.text = data.manaCost;
		typeLabel.text = data.type;
		textLabel.text = data.text;

		frameSprite.spriteName = CardRenderTool.ParseFrame(data);
	}
}