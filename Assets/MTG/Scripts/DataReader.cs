using System;
using Newtonsoft.Json;
using UnityEngine;
using ZFrame.IO;

public class DataReader : MonoBehaviour
{
	private void Start()
	{
		SetData data = JsonConvert.DeserializeObject<SetData>(ResourceEngine.Instance.Load<TextAsset>("FRF").text);

		if (data != null)
		{
			Debug.Log(data.name);
			Debug.LogError(data.code);
			Debug.LogWarning(data.releaseDate);
			Debug.LogException(new NotImplementedException("shit"));

			//foreach (object boo in data.booster)
			//{
			//	if (boo is ICollection)
			//	{
			//		foreach (object obj in (ICollection) boo)
			//		{
			//			Debug.Log(obj);
			//		}
			//	}
			//	else
			//	{
			//		Debug.Log(boo);
			//	}
			//}
		}
	}
}