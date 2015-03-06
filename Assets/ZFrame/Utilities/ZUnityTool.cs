using UnityEngine;

namespace ZFrame.Utilities
{
	public static class ZUnityTool
	{
		/// <summary>
		/// Change Target gameobject's layer
		/// </summary>
		/// <param name="go"></param>
		/// <param name="layer"></param>
		/// <param name="changeChildren">change all children</param>
		public static void ChangeLayer(GameObject go, int layer, bool changeChildren = true)
		{
			go.layer = layer;
			if (!changeChildren)
			{
				return;
			}
			foreach (Transform child in go.transform)
			{
				ChangeLayer(child.gameObject, layer);
			}
		}
	}
}