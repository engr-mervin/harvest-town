using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TouchControlsInventory : MonoBehaviour
{
    public RectTransform[] ignore;

    public Vector2 initialTouch;

    public ItemGrid itemGrid;
    public BtnInventory btnInv;

    public Vector3 originalCamPos;
    public float orthoSize;

    public bool applyCtnStarted = false;
    public bool startPan = false;
    public bool startZoom = false;
    public bool startApply = false;
    public enum State
    {
        Zoom,
        Pan,
        Apply
    };

    public State currentState;

    public List<UniqueTouch> touchList;

    //describes the extents of the touch control panel

    //on update

    public void OnDisable()
    {
        StopCoroutine("CtnApply");
        applyCtnStarted = false;

        if (touchList != null)
            touchList.Clear();

        originalCamPos = Vector3.zero;
    }
    private void Awake()
    {
        touchList = new List<UniqueTouch>();
    }

    private void Update()
    {
        TouchTracker();

        if (GetNumberOfTouches() == 0)
        {
            return;
        }
        if (!applyCtnStarted) //safeguard to make sure that only one coroutine runs
        {
            StartCoroutine(CtnApply());
        }
    }
    private void Pan(UniqueTouch ut)
    {
        float intensity = (ScreenSize.x / Screen.width) * 0.08f * (Camera.main.orthographicSize / 10f);
        Vector3 delta = ut.locationCurrent - initialTouch;
        Camera.main.transform.position = originalCamPos - delta * intensity;
    }

    private void Zoom(UniqueTouch ut1, UniqueTouch ut2)//only one will run
    {
        Vector3 deltaFirst = ut2.locationFirst - initialTouch;

        Vector3 deltaCurrent = ut2.locationCurrent - ut1.locationCurrent;

        float d = (deltaCurrent.magnitude - deltaFirst.magnitude) / 150f;

        print(d);

        if (10f > orthoSize - d && orthoSize - d > 4f)
            Camera.main.orthographicSize = orthoSize - d;
        else
            return;
    }

    private void CloseInv()
    {
        btnInv.CloseInventoryFunc();
    }


    IEnumerator CtnApply()
    {
        applyCtnStarted = true;
        initialTouch = touchList[0].locationFirst;

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (GetNumberOfTouches() == 0 && !startPan && !startZoom && !startApply) //if touch is released, apply the tile, one way
            {
                startApply = true;
                CloseInv();
                startApply = false;
                applyCtnStarted = false;
                yield break;
            }
            else if (GetNumberOfTouches() == 1 && touchList[0].locationCurrent != touchList[0].locationFirst) //if touch is moved, pan
            {
                if (!startPan && startZoom)
                {
                    startPan = true;
                    startZoom = false;
                    originalCamPos = Camera.main.transform.position;
                    initialTouch = touchList[0].locationCurrent;
                }
                if (!startPan && !startZoom)
                {
                    startPan = true;
                    originalCamPos = Camera.main.transform.position;
                    initialTouch = touchList[0].locationFirst;
                }

                Pan(touchList[0]);
                yield return new WaitForEndOfFrame();
            }
            else if (GetNumberOfTouches() == 2) //if another touch is added, zoom
            {
                if (!startZoom && startPan)
                {
                    startZoom = true;
                    startPan = false;
                    initialTouch = touchList[0].locationCurrent;
                    orthoSize = Camera.main.orthographicSize;
                }
                if (!startZoom && !startPan)
                {
                    startZoom = true;
                    orthoSize = Camera.main.orthographicSize;
                }

                Zoom(touchList[0], touchList[1]);
                yield return new WaitForEndOfFrame();

            }
            else if (GetNumberOfTouches() == 0) //all fingers released
            {
                startZoom = false;
                startPan = false;
                applyCtnStarted = false;
                yield break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private int GetNumberOfTouches()
    {
        return touchList.Count;
    }

    private void TouchTracker() //adds to list the touches that are inside the extents, saves the first location
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)//add touch
            {
                Touch t = Input.GetTouch(i);
                if (IsValid(t))
                {
                    UniqueTouch ut = new UniqueTouch(t, t.fingerId, t.position);
                    ut.locationCurrent = t.position;
                    touchList.Add(ut);
                    print("Added @ " + t.position);
                }
            }
            if (Input.GetTouch(i).phase != TouchPhase.Began)//update touch
            {
                Touch t = Input.GetTouch(i);
                if (IsValid(t))
                {
                    UniqueTouch ut = touchList.Find(ct => ct.touchID == t.fingerId);
                    if (ut == null) continue;
                    ut.movedDir = t.position - ut.locationCurrent;
                    ut.locationCurrent = t.position;
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended)//remove touch
            {
                Touch t = Input.GetTouch(i);
                UniqueTouch ut = touchList.Find(ct => ct.touchID == t.fingerId);
                if (ut == null) continue;
                touchList.Remove(ut);
                print("Removed");
            }

        }
    }

    private bool IsValid(Touch t)
    {
        if (IsInside(t.position, itemGrid.bottomLeft, itemGrid.topRight))
            return false;

        foreach (RectTransform b in ignore)
        {
            if (b == null)
                continue;
            if (!b.gameObject.activeInHierarchy)
                continue;
            Vector2 bl = MyFunctions.BottomLeft(b.localPosition, b.sizeDelta, 1.50f);
            Vector2 tr = MyFunctions.TopRight(b.localPosition, b.sizeDelta, 1.50f);

            if (IsInside(t.position, bl, tr))
                return false;
        }
        return true;

    }
    private bool IsInside(Vector2 pos, Vector2 bottomLeft, Vector2 topRight) //screen position
    {
        if (pos.x < topRight.x && pos.x > bottomLeft.x && pos.y < topRight.y && pos.y > bottomLeft.y)
            return true;
        else
            return false;
    }
    //wait for a touch that is within the extents

    //there are three types of touch control

    //number one is zoom, characterized by two touches, the two touches move away or towards each other

    //number two is panning, characterized by two touches, the two touches move in a single direction

    //number three is tile application, characterized by one touch
}
