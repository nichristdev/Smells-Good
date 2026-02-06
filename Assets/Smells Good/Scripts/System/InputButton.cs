using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressed;
    public Image img;
    public Color startColor;
    public Color pressedColor;
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        img.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        img.color = startColor;
    }
}
