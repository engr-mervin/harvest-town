using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
public static class MyFunctions
{
  public static Vector3 ScreenToWorld(Vector2 vector2, Transform camera)
    {
        Vector3 screenPoint = new Vector3(vector2.x, vector2.y, 0f);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        Vector3 worldPointZAdjusted = worldPoint + new Vector3(0f, 0f, screenPoint.z-camera.position.z);
        return worldPointZAdjusted;
    }

    public static Vector3 WorldToScreen(Vector2 vector2, Transform camera)
    {
        Vector3 worldPoint = new Vector3(vector2.x, vector2.y, 0f);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
        return screenPoint;
    }

    public static LayerMask LayerToLayerMask(int layer)
    {
        LayerMask layerMask = 1 << layer;
        return layerMask;
    }

    public static int LayerMaskToLayer(LayerMask layerMask)
    {
        int i = 0;
        while(layerMask!=0)
        {
            i++;
            layerMask >>= 1;
        }
        return i;
    }

    public static bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        LayerMask equiLayerMask = 1 << layer;
        LayerMask overlap = equiLayerMask | layerMask;
        if (overlap == layerMask)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool LayerMaskContainsLayerMask(LayerMask layerMask1, LayerMask layerMask2)
    {
        LayerMask overlap = layerMask1 | layerMask2;
        if (overlap == layerMask1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static Vector3Int Vector2IntToTileLocation(Vector2Int pos)
    {
        return new Vector3Int(pos.x, pos.y, 0);
    }

    public static Vector3 Vector2toVector3(Vector2Int vec)
    {
        Vector3 worldPos = new Vector3(vec.x, vec.y, 0);
        return worldPos;
    }
    public static Vector2Int TransformtoVector2Int(Vector3 vec)
    {
        Vector2Int worldPos = new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
        return worldPos;
    }

    public static Vector2Int TransformtoVector2Int(Vector3 vec,Vector2 offset)
    {
        Vector2Int worldPos = new Vector2Int(Mathf.FloorToInt(vec.x+offset.x), Mathf.FloorToInt(vec.y+offset.y));
        return worldPos;
    }

    public static Vector3 Vector2InttoTransform(Vector2Int vec)
    {
        Vector3 worldPos = new Vector3(vec.x+0.50f, vec.y+0.50f, 0);
        return worldPos;
    }

    public static Vector3Int Vector3toVector3Int(Vector3 vec)
    {
        Vector3Int res = new Vector3Int();
        res.x = Mathf.FloorToInt(vec.x);
        res.y = Mathf.FloorToInt(vec.y);
        res.z = Mathf.FloorToInt(vec.z);

        return res;
    }

    public static Vector2 RectTransformtoScreenPosition(Vector2 rt)
    {
        float w = ScreenSize.x;
        float h = ScreenSize.y;

        float percentX = 0.50f + (rt.x / w);
        float percentY = 0.50f + (rt.y / h);

        Vector2 res = new Vector2();

        res.x = percentX * Screen.width;
        res.y = percentY * Screen.height;

        return res;
    }

    public static Vector2 ScreenPositionToRectTransform(Vector3 sPos)
    {
        float x = ((sPos.x / Screen.width) - 0.50f) * 800f;
        float y = ((sPos.y / Screen.height) - 0.50f) * 400f;

        return new Vector2(x,y);
    }

    public static Vector2 RectSizetoScreenSize(Vector2 size)
    {
        float w = ScreenSize.x;
        float h = ScreenSize.y;

        Vector2 res = new Vector2();

        res.x = size.x * Screen.width/w;
        res.y = size.y * Screen.height/h;

        return res;
    }


    public static Vector2 BottomLeft(Vector2 pos, Vector2 size,float FOS=1f)//with 20% F.O.S.
    {
        return MyFunctions.RectTransformtoScreenPosition(pos) - (MyFunctions.RectSizetoScreenSize(size)*FOS / 2.0f);
    }

    public static Vector2 TopRight(Vector2 pos, Vector2 size, float FOS = 1f)
    {
        return MyFunctions.RectTransformtoScreenPosition(pos) + (MyFunctions.RectSizetoScreenSize(size)*FOS / 2.0f);
    }

    public static bool ScreenPosIsInsideRectTransform(Vector3 screenPos, RectTransform rt, float FOS = 1f)
    {
        Vector3 worldPos = rt.GetComponent<UI_Position>().worldPos;
        Vector2 bl = BottomLeft(worldPos, rt.sizeDelta,FOS);
        Vector2 tr = TopRight(worldPos, rt.sizeDelta,FOS);

        if (screenPos.x < tr.x && screenPos.x > bl.x && screenPos.y < tr.y && screenPos.y > bl.y)
                return true;
            else
                return false;
    }
    public static bool IsInside(Vector2 pos, Vector2 bottomLeft, Vector2 topRight) //is a point inside rectangular area defined by two points
    {
        if (pos.x < topRight.x && pos.x > bottomLeft.x && pos.y < topRight.y && pos.y > bottomLeft.y)
            return true;
        else
            return false;
    }

    public static Vector3Int TouchtoTilePos(Vector2 touch)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(touch);
        int x = Mathf.FloorToInt(point.x);
        int y = Mathf.FloorToInt(point.y);
        int z = 0;

        return new Vector3Int(x,y,z);
    }

    public static void ToggleBool(ref bool toggle)
    {
        if(toggle)
        {
            toggle = false;
        }
        else
        {
            toggle = true;
        }
    }
    
    public static void HideObjects(GameObject[] hide)
    {
        foreach(GameObject g in hide)
        {
            g.SetActive(false);
        }
    }
    public static void ShowObjects(GameObject[] hide)
    {
        foreach (GameObject g in hide)
        {
            g.SetActive(true);
        }
    }

    public static void ToggleObjects(GameObject[] toggle)
    {
        foreach (GameObject g in toggle)
        {
            if (g.activeSelf)
                g.SetActive(false);
            else
                g.SetActive(true);
        }
    }

    public static Vector2 BottomLeftToCenterLocation(Vector2 bl)
    {
        float x = bl.x - 0.50f;
        float y = bl.y - 0.50f;

        return new Vector2(x, y);
    }


    public static void TintButton(Button button,float tint)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, tint);
    }
    public static void UntintButton(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1.0f);
    }

    public static void Toggle(GameObject[] enable, GameObject[] disable, GameObject[] disable2 = null)
    {
        foreach (GameObject d in disable)
        {
            d.SetActive(false);
        }
        foreach (GameObject e in enable)
        {
            e.SetActive(true);
        }
        if (disable2 != null)
        {
            foreach (GameObject d in disable2)
            {
                d.SetActive(false);
            }
        }
    }

    public static void SingleToggle(GameObject enable, GameObject disable)
    {
        if(disable!=null)
            disable.SetActive(false);

        if (enable != null)
            enable.SetActive(true);
       
    }
}
