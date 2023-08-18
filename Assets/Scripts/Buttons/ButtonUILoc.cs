using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUILoc
{
    public Vector2 position;
    private Vector2 bottomLeft;

    public ButtonUILoc(Vector2Int location)
    {
        bottomLeft.x = -ScreenSize.x / 2;
        bottomLeft.y = -ScreenSize.y / 2;



        position.x = bottomLeft.x + location.x * ScreenSize.x;
        position.y = bottomLeft.y + location.y * ScreenSize.y;
    }
}
