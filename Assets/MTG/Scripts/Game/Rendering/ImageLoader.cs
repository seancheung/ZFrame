using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
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

	public void LoadAsync(UITexture texture, string multiverseid)
	{
		StartCoroutine(File.Exists(_path + multiverseid)
			? LoadLoacl(texture, multiverseid)
			: CacheLocal(texture, multiverseid));
	}

	private IEnumerator LoadLoacl(UITexture texture, string multiverseid)
	{
		string filePath = "file:///" + _path + multiverseid;
		WWW www = new WWW(filePath);
		yield return www;

		texture.mainTexture = www.texture;
	}

	private IEnumerator CacheLocal(UITexture texture, string multiverseid)
	{
		WWW www = new WWW(string.Format("http://mtgimage.com/multiverseid/{0}.jpg", multiverseid));
		yield return www;

		byte[] img = www.texture.EncodeToJPG();
		File.WriteAllBytes(_path + multiverseid, img);

		texture.mainTexture = www.texture;
	}

	private IEnumerator LoadZip(UITexture texture, string setCode, string name)
	{
		FileStream fs = File.OpenRead(_path + setCode + ".zip");
		ZipFile zip = new ZipFile(fs);
		int index = zip.FindEntry(name, true);
		yield return index;

		if (index > 0)
		{
			ZipEntry entry = zip[index];
			Stream stream = zip.GetInputStream(entry);
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, (int) stream.Length);
			Texture2D t2d = new Texture2D(texture.width, texture.height);
			t2d.LoadImage(buffer);
			File.WriteAllBytes(_path + setCode + "/" + name, buffer);
			texture.mainTexture = t2d;
			yield return texture;
		}
	}
}