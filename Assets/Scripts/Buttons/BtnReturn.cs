using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnReturn : MonoBehaviour, IPointerClickHandler
{
    public MovingObject mo;

    public void OnPointerClick(PointerEventData eventData)
    {
        mo.MoveToOriginal();
    }
}
