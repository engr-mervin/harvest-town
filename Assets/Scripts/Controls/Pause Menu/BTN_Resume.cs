﻿using UnityEngine;
using UnityEngine.EventSystems;

public class BTN_Resume : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private Buttons_PauseMenu pm;

    public void OnPointerClick(PointerEventData eventData)
    {
        pm.Resume();
    }

}
