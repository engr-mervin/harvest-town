using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingWallsState : PlayerState
{
    // Start is called before the first frame update
    public AddingWallsState(PlayerStateManager psm, PlayerMovement actorMove,GameObject adder)//call when creating new State
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.addingWalls != null)
        {
            StatePanel.addingWalls.SetActive(true);
        }

        psm.currentPanel = StatePanel.addingWalls;
        psm.HotBar(this);
        //camera controls
        Camera.main.GetComponent<FN_FollowPlayer>().StopFollow();
        //additional logic
        actorMove.StopMovement();
    }

}
