using UnityEngine;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform bar;
    public RectTransform button;

    private float _left;
    private float _right;

    void Start()
    {
        _left = bar.position.x - bar.rect.width / 2f + button.rect.width / 2f;
        _right = bar.position.x + bar.rect.width / 2f - button.rect.width / 2f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = button.position;
        pos.x = Mathf.Clamp(eventData.position.x, _left, _right);
        button.position = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = button.position;
        pos.x = Mathf.Clamp(eventData.position.x, _left, _right);
        button.position = pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        button.localPosition = Vector3.zero;
    }
}