using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ZResource : MonoBehaviour
{
	[Serializable]
	public class Resource
	{
		public string key;
		public Object resource;

		public static implicit operator GameObject(Resource resource)
		{
			return resource.resource as GameObject;
		}

		public static implicit operator Object(Resource resource)
		{
			return resource.resource;
		}
	}

	public Resource[] resources;

	public Resource this[string key]
	{
		get { return resources == null ? null : resources.FirstOrDefault(r => r.key == key); }
	}
}