using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_Movement_Secondary : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2Int direction;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GM.playerMove == null) return;

        GM.playerMove.MovePlayer(direction);

        MyFunctions.TintButton(this.gameObject.GetComponent<Button>(),0.60f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GM.playerMove == null) return;

        GM.playerMove.ExitPrimary(direction);

        MyFunctions.UntintButton(this.gameObject.GetComponent<Button>());
    }
}
