using System.Collections;
using System.IO;
using UnityEngine;
using ZFrame.MonoBase;

public class AsyncImageDownloader : MonoSingleton<AsyncImageDownloader>
{
	public Texture placeholder;

	private readonly string _path = Application.persistentDataPath + "/ImageCache/";

	protected void Start()
	{
		if (!Directory.Exists(Application.persistentDataPath + "/ImageCache/"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/ImageCache/");
			placeholder = Resources.Load("placeholder") as Texture;
		}
	}


	public void SetAsyncImage(string url, UITexture texture)
	{
		texture.mainTexture = placeholder;

		StartCoroutine(!File.Exists(_path + url.GetHashCode()) ? DownloadImage(url, texture) : LoadLocalImage(url, texture));
	}

	private IEnumerator DownloadImage(string url, UITexture texture)
	{
		Debug.Log("downloading new image:" + _path + url.GetHashCode());
		WWW www = new WWW(url);
		yield return www;

		Texture2D image = www.texture;

		byte[] pngData = image.EncodeToPNG();
		File.WriteAllBytes(_path + url.GetHashCode(), pngData);

		texture.mainTexture = image;
	}

	private IEnumerator LoadLocalImage(string url, UITexture texture)
	{
		string filePath = "file:///" + _path + url.GetHashCode();

		Debug.Log("getting local image:" + filePath);
		WWW www = new WWW(filePath);
		yield return www;

		texture.mainTexture = www.texture;
	}
}