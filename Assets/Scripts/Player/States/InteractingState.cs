using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingState : PlayerState
{
    public InteractingState(PlayerStateManager psm, PlayerMovement actorMove)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.interacting != null)
        {
            StatePanel.interacting.SetActive(true);
        }
        psm.currentPanel = StatePanel.interacting;
        psm.HotBar(this);
        //camera controls

        actorMove.StopMovement();

        StatePanel.interacting.GetComponent<Interacting>().SpawnFirstSet(actorMove);
    }

    public static bool CanInteract(PlayerMovement actorMove)
    {
        GameObject[] objects = InteractRayCast.GetAllObjects(actorMove);

        //IF NONE THEN DON'T DO ANYTHING
        if (objects == null || objects.Length == 0)
            return false;

        return true;
    }
}
