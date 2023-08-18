using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GM.playerMove == null)
            return;

        if (InteractingState.CanInteract(GM.playerMove))
            GM.playerState.SetState(new InteractingState(GM.playerState,GM.playerMove));

    }
}
