using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
	public Image image;

	public CardData data;

	void Awake()
	{
		data = null;
	}

	private void Start()
	{
		if (data != null)
		{
			ImageLoader.Instance.LoadAsync(image, data.multiverseid);
		}
	}
}