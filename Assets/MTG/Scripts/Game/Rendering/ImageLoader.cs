using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZFrame.MonoBase;

public class ImageLoader : MonoSingleton<ImageLoader>
{
	private readonly string _path = Application.persistentDataPath + "/ImageCache/";

	private void Start()
	{
		Debug.Log(_path);
		if (!Directory.Exists(Application.persistentDataPath + "/ImageCache/"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/ImageCache/");
		}
	}

	public void LoadAsync(Image image, string multiverseid)
	{
		StartCoroutine(File.Exists(_path + multiverseid)
			? LoadLoacl(image, multiverseid)
			: CacheLocal(image, multiverseid));
	}

	private IEnumerator LoadLoacl(Image image, string multiverseid)
	{
		string filePath = "file:///" + _path + multiverseid;
		WWW www = new WWW(filePath);
		yield return www;

		image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
	}

	private IEnumerator CacheLocal(Image image, string multiverseid)
	{
		WWW www = new WWW(string.Format("http://mtgimage.com/multiverseid/{0}.jpg", multiverseid));
		yield return www;

		byte[] img = www.texture.EncodeToJPG();
		File.WriteAllBytes(_path + multiverseid, img);

		image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
	}
}