using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingFloorsState : PlayerState
{
    // Start is called before the first frame update
    public AddingFloorsState(PlayerStateManager psm, PlayerMovement actorMove)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.addingFloors != null)
        {
            StatePanel.addingFloors.SetActive(true);
        }

        psm.currentPanel = StatePanel.addingFloors;
        psm.HotBar(this);
        //camera controls
        Camera.main.GetComponent<FN_FollowPlayer>().StopFollow();
        //additional logic
        actorMove.StopMovement();
    }
}
