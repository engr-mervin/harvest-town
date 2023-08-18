using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_Movement_Primary : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2Int direction;
    [SerializeField]
    private UI_Movement_DPad dPad;

    public void OnPointerDown(PointerEventData eventData) //on first touch down
    {
        if (GM.playerMove == null) return;

        dPad.ShowSecondaryButtons();
        GM.playerMove.ButtonMove(direction);
    }
    public void OnPointerUp(PointerEventData eventData) // on touch up
    {
        if (GM.playerMove == null) return;

        dPad.HideSecondaryButtons();
        GM.playerMove.StopMovement();
        dPad.ResetTint();
    }
    public void OnPointerEnter(PointerEventData eventData) //on change key
    {
        if (GM.playerMove == null) return;

        GM.playerMove.MovePlayer(direction);
        MyFunctions.TintButton(this.gameObject.GetComponent<Button>(),StaticValues.buttonAlphaTint);
    }
    public void OnPointerExit(PointerEventData eventData) //on key exit
    {
        if (GM.playerMove == null) return;

        GM.playerMove.ExitPrimary(direction);
        MyFunctions.UntintButton(this.gameObject.GetComponent<Button>());
    }
}
