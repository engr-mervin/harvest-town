using UnityEngine;

public class MovementState : PlayerState
{
    public MovementState(PlayerStateManager psm)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel!=null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.movement != null)
        {
            StatePanel.movement.SetActive(true);
        }

        psm.currentPanel = StatePanel.movement;
        psm.HotBar(this);
        //camera controls
        Camera.main.GetComponent<FN_FollowPlayer>().PlayerFollow(GM.playerObj.transform);
    }
}
