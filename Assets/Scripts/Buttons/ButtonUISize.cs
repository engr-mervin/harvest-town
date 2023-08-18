using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUISize
{
    public Vector2 size;

    public ButtonUISize(Vector2Int _size)
    {
        size.x = _size.x * ScreenSize.x;
        size.y = _size.y * ScreenSize.y;
    }
}
