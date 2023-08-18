using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_AddFloor : BC_ActionOption
{
    public override void ButtonClicked()
    {
        AddFloor();
    }

    private void AddFloor()
    {
        //****check if able to modify awlls first
        GM.playerState.SetState(new AddingFloorsState(GM.playerState,GM.playerMove));

    }
}
