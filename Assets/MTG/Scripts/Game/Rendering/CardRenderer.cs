using UnityEngine;

public class CardRenderer : MonoBehaviour
{
	public UITexture texture;

	public CardData data;

	private void Start()
	{
		if (data != null)
		{
			ImageLoader.Instance.LoadAsync(texture, data.multiverseid);
		}
	}
}