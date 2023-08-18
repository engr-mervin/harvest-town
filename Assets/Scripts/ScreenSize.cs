using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenSize //computes screen height and width
{
    public static float x { get { return _x; } }
    public static float y { get {return _y; } }
    public static Vector2 xy { get { return _xy; } }


    private static float _x;
    private static float _y;
    private static Vector2 _xy;

    static ScreenSize()
    {
        //Reference resolution = 800*600 (width*height)
        //matches width so x=800

        _x = 800f;
        _y = 800f * Camera.main.pixelHeight / Camera.main.pixelWidth;

        _xy = new Vector2(_x, _y);
    }
}
