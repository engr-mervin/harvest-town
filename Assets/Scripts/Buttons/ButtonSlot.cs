using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSlot : MonoBehaviour, IPointerClickHandler
{
    public int slot;

    [SerializeField]
    private Buttons_PauseMenu pm;

    public void OnPointerClick(PointerEventData eventData)
    {
    }

}
