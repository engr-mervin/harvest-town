using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnObjectMove : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public Vector2Int direction;
    public MovingObject mo;

    public void OnPointerClick(PointerEventData eventData)
    {
         mo.Move(mo.objectTrans.pivot + direction);
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MyFunctions.UntintButton(this.gameObject.GetComponent<Button>());
    }
}
