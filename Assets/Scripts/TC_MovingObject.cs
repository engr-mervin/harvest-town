using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TC_MovingObject : BC_TouchControls
{
    public MovingObject mo;

    public Text text;

    internal override void OnDisable()
    {
        base.OnDisable();

        text.text = "";
    }

    internal override void SingleTouch()
    {
        base.SingleTouch();
        MoveHere();
    }
    private void MoveHere()
    {
        GameObject move = mo.moveObject;

        if (move == null)
            return;

        Vector2Int pointA2 = Positions.TouchToBlockPos(firstTouch);


        bool compatible = move.GetComponent<Placeable>().CanMoveToBlock(pointA2);

        if (!compatible)
        {
            text.text = "Can't place here.";
            return;
        }
        else
        {
            mo.Move(pointA2);

            if(mo.IsAllGreen()==true)
                text.text = "";
            else
                text.text = "Can't place here.";
        }

    }


}
