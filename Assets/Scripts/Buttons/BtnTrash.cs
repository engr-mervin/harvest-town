using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnTrash : MonoBehaviour, IPointerClickHandler
{
    public MovingObject mo;
    public void OnPointerClick(PointerEventData eventData)
    {
        mo.TakeObject();
    }
}
