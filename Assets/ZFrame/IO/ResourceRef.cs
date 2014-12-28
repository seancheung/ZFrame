using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZFrame.IO
{
	public class ResourceRef : MonoBehaviour
	{
		[Serializable]
		public class GameResource
		{
			public string key;
			public Object resource;

			public static implicit operator GameObject(GameResource gameResource)
			{
				return gameResource.resource as GameObject;
			}

			public static implicit operator Object(GameResource gameResource)
			{
				return gameResource.resource;
			}
		}

		[Serializable]
		public class ResourceGroup
		{
			public int id;
			public bool folded;
			public GameResource[] resources;
		}

		public ResourceGroup[] groups;

		public GameResource this[string key]
		{
			get
			{
				return
					groups.Select(resourceGroup => resourceGroup.resources.FirstOrDefault(r => r.key == key))
						.FirstOrDefault(res => res != null);
			}
		}
	}
}