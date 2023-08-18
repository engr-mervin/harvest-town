using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : PlayerState
{
    public PauseState(PlayerStateManager psm)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);



        if (StatePanel.paused != null)
        {
            StatePanel.paused.SetActive(true);
        }

        psm.currentPanel = StatePanel.paused;
        psm.HotBar(this);
        //camera controls
        GM.playerMove.StopMovement();
    }
}
