using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BC_TouchControls : MonoBehaviour
{
    public RectTransform[] ignore;

    public GameObject target;

    [SerializeField]
    internal Mode mode;

    internal enum Mode
    {
        None,
        SingleTouch,
        SingleTouchMoved,
        DoubleTouchOpp,
    };

    internal Vector2 touchPosLastFrame;

    internal Vector2 firstTouch;
    internal float firstOrtho;
    internal Vector3 firstCamPos;
    [SerializeField]
    internal bool shouldFollow;

    internal bool applyCtnStarted = false;
    internal List<UniqueTouch> touches = new List<UniqueTouch>();


    public virtual void ClearStaticList()
    {
        return;
    }
    public virtual void Subscribe()
    {
        return;
    }
    public virtual void Unsubscribe()
    {
        return;
    }
    internal void Unfollow()
    {
        if (shouldFollow == false) return;

        FN_FollowPlayer fp = Camera.main.GetComponent<FN_FollowPlayer>();
        
        fp.Unfollow();
    }

    internal void Follow()
    {
        if (shouldFollow == false) return;

        FN_FollowPlayer fp = Camera.main.GetComponent<FN_FollowPlayer>();
        
        fp.Follow();
    }

    internal IEnumerator TouchCoroutine()
    {
        Unfollow();
        float waitTime = 0.0f;
        applyCtnStarted = true;

        firstTouch = touches[0].locationFirst;
        firstOrtho = Camera.main.orthographicSize;

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (touches.Count == 0 && mode == Mode.None) //if touch is released, apply the tile, one way
            {
                SingleTouch();
                applyCtnStarted = false;
                mode = Mode.None;
                Follow();
                yield break;
            }

            else if (touches.Count == 1 && touches[0].locationCurrent != touches[0].locationFirst) //if touch is moved, pan
            {
                if (mode == Mode.DoubleTouchOpp) //from zooming
                {
                    firstTouch = touches[0].locationCurrent;
                }
                if (mode != Mode.SingleTouchMoved)
                {
                    firstCamPos = Camera.main.transform.position;
                }

                SingleTouchMoved(waitTime);
                touchPosLastFrame = touches[0].locationCurrent;
                yield return new WaitForEndOfFrame();
            }

            else if (touches.Count == 2) //if another touch is added, zoom
            {
                if (mode == Mode.SingleTouchMoved) //from panning
                {
                    firstTouch = touches[0].locationCurrent;
                }
                if (mode != Mode.DoubleTouchOpp)
                {
                    firstOrtho = Camera.main.orthographicSize;
                }

                DoubleTouchOpp();

                yield return new WaitForEndOfFrame();
            }

            else if (touches.Count == 0) //all fingers released
            {
                Follow();
                FingersReleased();
                mode = Mode.None;
                applyCtnStarted = false;
                yield break;
            }

            else if (touches.Count == 1 && touches[0].locationCurrent == touches[0].locationFirst)
            {
                if (mode == Mode.None)
                    waitTime+=Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            else //ON HOLD
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    internal virtual void FingersReleased()
    {
        return;
    }    

    internal void TouchTracker(RectTransform[] ignore, ref List<UniqueTouch> touches) //adds to list the touches that are inside the extents, saves the first location
    {
        if (Input.touchCount == 0) return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)//add touch
            {
                Touch t = Input.GetTouch(i);
                if (IsValid(t, ignore))
                {
                    UniqueTouch ut = new UniqueTouch(t, t.fingerId, t.position);
                    ut.locationCurrent = t.position;
                    touches.Add(ut);
                }
            }
            if (Input.GetTouch(i).phase != TouchPhase.Began)//update touch
            {
                Touch t = Input.GetTouch(i);
                if (IsValid(t, ignore))
                {
                    UniqueTouch ut = touches.Find(ct => ct.touchID == t.fingerId);
                    if (ut == null) continue;
                    ut.movedDir = t.position - ut.locationCurrent;
                    ut.locationCurrent = t.position;
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended)//remove touch
            {
                Touch t = Input.GetTouch(i);
                UniqueTouch ut = touches.Find(ct => ct.touchID == t.fingerId);
                if (ut == null) continue;
                touches.Remove(ut);
            }
        }
    }

    internal bool IsValid(Touch t, RectTransform[] ignore)
    {
        if (ignore.Length == 0) return true;

        foreach (RectTransform b in ignore)
        {
            if (b==null||!b.gameObject.activeSelf||b.GetComponent<UI_Position>()==null) continue;

            Vector2 bl = MyFunctions.BottomLeft(b.GetComponent<UI_Position>().worldPos, b.sizeDelta, 1.50f);
            Vector2 tr = MyFunctions.TopRight(b.GetComponent<UI_Position>().worldPos, b.sizeDelta, 1.50f);


            if (MyFunctions.IsInside(t.position, bl, tr))
                return false;
        }
        return true;
    }

    internal void Zoom(UniqueTouch ut1, UniqueTouch ut2, Vector2 initialTouch, float orthoSize)
    {
        Vector3 deltaFirst = ut2.locationFirst - initialTouch;

        Vector3 deltaCurrent = ut2.locationCurrent - ut1.locationCurrent;

        float d = (deltaCurrent.magnitude - deltaFirst.magnitude) / 150f;

        if (10f > orthoSize - d && orthoSize - d > 4f)
            Camera.main.orthographicSize = orthoSize - d;
        else
            return;
    }

    internal void Pan(UniqueTouch ut, Vector2 initialTouch, Vector3 originalCamPos)
    {
        float intensity = (ScreenSize.x / Screen.width) * 0.08f * (Camera.main.orthographicSize / 10f);
        Vector3 delta = ut.locationCurrent - initialTouch;
        Camera.main.transform.position = originalCamPos - delta * intensity;
    }



    internal virtual void SingleTouch()
    {
        mode = Mode.SingleTouch;
        return;
    }


    internal virtual void SingleTouchMoved(float wait)
    {
        mode = Mode.SingleTouchMoved;

        Pan(touches[0], firstTouch, firstCamPos);
    }

    internal virtual void DoubleTouchOpp()
    {
        mode = Mode.DoubleTouchOpp;

        Zoom(touches[0], touches[1], firstTouch, firstOrtho);
    }

    internal virtual void OnDisable()
    {
        StopCoroutine(nameof(TouchCoroutine));
        applyCtnStarted = false;
        mode = Mode.None;


        if (touches != null)
            touches.Clear();

        firstCamPos = Vector3.zero;
    }


    internal virtual void Update()
    {
        TouchTracker(ignore, ref touches);

        if (touches.Count != 0 && !applyCtnStarted)
        {
            StartCoroutine(TouchCoroutine());
        }
        
    }

}
