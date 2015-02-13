using UnityEngine;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour, IDragHandler
{
	#region Implementation of IDragHandler

	public void OnDrag(PointerEventData eventData)
	{
		GetComponent<RectTransform>().pivot.Set(0, 0);
		transform.position = Input.mousePosition;
	}

	#endregion
}