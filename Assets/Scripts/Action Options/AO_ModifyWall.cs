using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_ModifyWall : BC_ActionOption
{
    public override void ButtonClicked()
    {
        ModifyWalls();
    }

    private void ModifyWalls()
    {
        //****check if able to modify awlls first
        GM.playerState.SetState(new AddingWallsState(GM.playerState,GM.playerMove,gameObject));
        return;
    }
}
