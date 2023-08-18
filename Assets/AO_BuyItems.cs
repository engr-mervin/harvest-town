using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_BuyItems : BC_ActionOption
{
    [SerializeField]
    public List<BC_SellItems> forSale = new List<BC_SellItems>();

    public override void ButtonClicked()
    {
        GM.playerState.SetState(new BuyingState(GM.playerState,GM.playerMove, gameObject));
    }
}
