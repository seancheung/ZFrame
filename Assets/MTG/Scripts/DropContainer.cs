using UnityEngine;
using UnityEngine.EventSystems;

public class DropContainer : MonoBehaviour, IDropHandler
{

	#region Implementation of IDropHandler

	public void OnDrop(PointerEventData eventData)
	{
		eventData.pointerDrag.transform.SetParent(transform,false);
	}

	#endregion
}