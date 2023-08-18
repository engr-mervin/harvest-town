using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingState : PlayerState
{
    public BuyingState(PlayerStateManager psm, PlayerMovement buyer,GameObject seller)
    {
        psm.currentState = this;
        psm.SetTimeScale(this);

        if (psm.currentPanel != null)
            psm.currentPanel.SetActive(false);


        if (StatePanel.buying != null)
        {
            StatePanel.buying.SetActive(true);
        }

        psm.currentPanel = StatePanel.buying;
        psm.HotBar(this);
        //camera controls
        buyer.StopMovement();

        StatePanel.buying.GetComponent<Buying>().InstantiateItems(seller);
    }
}
