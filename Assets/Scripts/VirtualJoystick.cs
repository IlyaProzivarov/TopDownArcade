using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform handle;
    public float radius = 90f;
    public Vector2 Direction { get; private set; }

    RectTransform baseRect;

    void Awake()
    {
        baseRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData e) { OnDrag(e); }

    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseRect, e.position, e.pressEventCamera, out Vector2 local);
        Vector2 clamped = Vector2.ClampMagnitude(local, radius);
        handle.anchoredPosition = clamped;
        Direction = clamped / radius;
    }

    public void OnPointerUp(PointerEventData e)
    {
        handle.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
    }
}