using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectState : PlayerState
{
    public MovingObjectState(PlayerStateManager psm, GameObject moveObject,MovingObject.Type moveType, MovingObject.Number movingCount = MovingObject.Number.Single)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.movingObject != null)
        {
            StatePanel.movingObject.SetActive(true);
        }

        psm.currentPanel = StatePanel.movingObject;
        psm.HotBar(this);
        //camera controls
        Camera.main.GetComponent<FN_FollowPlayer>().ObjectFollow(moveObject.transform);


        GM.playerMove.StopMovement();

        StatePanel.movingObject.GetComponent<MovingObject>().StartMoving(moveObject, GM.playerMove, moveType,movingCount);
    }
}
