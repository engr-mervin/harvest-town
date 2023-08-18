using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    
    public GameObject moveObject;
    public PlayerMovement mover;

    public Stackable stackable;
    public Placeable movable;
    public ObjectTransform objectTrans;
    public Rotatable rotatable;
    public Number movingCount;

    public string moveObjectCode;
    public GameObject markers;

    public List<GameObject> objectMarkers;

    public Vector2Int originalPivot;


    [Header("Set This")]
    public GameObject rotateButton;

    public enum Type
    {
        ExistingObject,
        NewObject
    };

    public enum Number
    {
        Single,
        Multiple
    };

    private Type movingType;

    public void Rotate()
    {
        if (rotatable == null) return;
        rotatable.RotateNext();
        DestroyMarkers();
        CreateMarkers();
        Move(objectTrans.pivot);
    }
    public void StartMoving(GameObject _moveObject, PlayerMovement _mover, Type _movingType, Number _movingCount=Number.Single)
    {
        moveObject = _moveObject;
        mover = _mover;
        movingType = _movingType;
        moveObjectCode = _moveObject.GetComponent<OBJ_ObjectSaveData>().itemIndex;
        movingCount = _movingCount;
        stackable = moveObject.GetComponent<Stackable>();
        movable = moveObject.GetComponent<Placeable>();
        objectTrans = moveObject.GetComponent<ObjectTransform>();
        rotatable = moveObject.GetComponent<Rotatable>();

        originalPivot = objectTrans.pivot;


        if (_movingCount == Number.Single)
        {//disable buttons
            if (rotatable == null)
                rotateButton.SetActive(false);
            else
                rotateButton.SetActive(true);
            //Remove Object
            if (movingType == Type.ExistingObject)
                S_WorldBlocks.RemoveObjectFromWorld(moveObject);

        }
        else
        {
            rotateButton.SetActive(false);

            foreach(Transform t in moveObject.GetComponentsInChildren<Transform>())
            {
                if (t.GetComponent<ObjectTransform>() == null)
                    continue;


                S_WorldBlocks.RemoveObjectFromWorld(t.gameObject);
            }
        }


        //create markers
        CreateMarkers();

        ColorMarkers();
        //Move
        if (movingType == Type.NewObject)
        {
            Move(originalPivot);
        }

    }

    private void OnDisable()
    {
        EndClearing();
    }
    private void DestroyMarkers()
    {
        foreach (GameObject g in objectMarkers)
        {
            Destroy(g);
        }
    }
    private void EndClearing()
    {
        DestroyMarkers();
        objectMarkers.Clear();

        moveObject = null;
        mover = null;
        stackable = null;
        movable = null;
        objectTrans = null;
    }

    public void CreateMarkers(float offsetX = 0f,float offsetY = 0f)
    {
        objectMarkers = new List<GameObject>();
        Vector2Int[] vc = objectTrans.AllBlockPoints(objectTrans.pivot);

        for(int i=0;i<vc.Length;i++)
        {
            GameObject g = Instantiate(markers);
            g.transform.localScale = Vector3.one * 0.50f;
            g.transform.position = Positions.BlockToTransform(vc[i])-new Vector3(0.25f,0.25f,0);
            objectMarkers.Add(g);
        }
    }

    public void ColorMarkers()//logic for object compatibility
    {
        Vector2Int[] vc = objectTrans.AllBlockPoints(objectTrans.pivot);
        for (int i = 0; i < vc.Length; i++)
        {
            Block b = S_WorldBlocks.GetBlockinPosition(vc[i]);
            if (stackable == null)
            {

                //not the same elevation
                if (b == null && objectTrans.botElev != 0)
                {
                    objectMarkers[i].GetComponent<Marker>().Red();
                    continue;
                }
                //not stackable but there is an object
                if (b != null)
                {
                    objectMarkers[i].GetComponent<Marker>().Red();
                    continue;
                }

            }
            if (stackable != null)
            {
                if (stackable.ColorStack(objectTrans.pivot,vc[i]))
                {
                    objectMarkers[i].GetComponent<Marker>().Green();
                    continue;
                }
                else
                {
                    objectMarkers[i].GetComponent<Marker>().Red();
                    continue;
                }
            }

            objectMarkers[i].GetComponent<Marker>().Green();
            continue;
        }
    }

    public void MoveMarkers(Vector2Int previousPivot,Vector2Int newPivot)//blockpositions
    {
        foreach(GameObject g in objectMarkers)
        {
            Vector2Int delta = newPivot - previousPivot;
            Vector2 deltaTrans = ((Vector2)(delta)) * 0.50f;
            g.transform.position += (Vector3)(deltaTrans);
        }
    }



    public void Move(Vector2Int blockPivot) //logic if object can move
    {
        //placeable but is fixed and is not the first placing
        if (movable.isFixed == true && movingType == Type.ExistingObject) return;

        //Check if object can MOVE(not place) to block
        if (!movable.CanMoveToBlock(blockPivot))
        {
            return;
        }

        Vector2Int delta = blockPivot - objectTrans.pivot;
        print(delta);
        MoveMarkers(objectTrans.pivot, blockPivot);
        objectTrans.MoveToBlock(blockPivot);
        movable.SetWallParent();
        ColorMarkers();
        
        if(movingCount==Number.Multiple)
        {
            foreach (Transform t in moveObject.GetComponentsInChildren<Transform>())
            {
                if (t.GetComponent<ObjectTransform>() == null)
                    continue;
                if (t.gameObject == moveObject)
                    continue;

                t.GetComponent<ObjectTransform>().MoveToBlockByDeltaChildren(delta);
            }
        }
    }
    public bool IsAllGreen()//Are all markers green?
    {
        foreach (GameObject g in objectMarkers)
        {
            if (g.GetComponent<Marker>().markerState == Marker.State.Red)
                return false;
        }
        return true;
    }



    public void EndMovePlace() //call when placing an object
    {
        if (!IsAllGreen()) return; //is all markers green?

        if (moveObject != null)
            S_WorldBlocks.PlaceObjectInWorld(objectTrans, objectTrans.pivot); //no layer index to place object to the topmost

        if(stackable!=null)
        {
            objectTrans.RefreshLayer();
        }

        foreach (Transform t in moveObject.GetComponentsInChildren<Transform>())
        {
            if (t.GetComponent<ObjectTransform>() == null)
                continue;
            if (t.gameObject == moveObject)
                continue;

            ObjectTransform current = t.GetComponent<ObjectTransform>();
            Stackable st = t.GetComponent<Stackable>();

            if (st != null)
            {
                st.Stack();
            }
            S_WorldBlocks.PlaceObjectInWorld(current, current.pivot);

            if (st != null)
            {
                current.RefreshLayer();
            }
        }

        EndMoveState();
    }

    public void EndMoveState()
    {
        EndClearing();
        AStar_Grid.instance.SetWalkableNodes();
        GM.playerState.SetState(new MovementState(GM.playerState));
    }

    //BUTTON LOGIC

    public void TakeObject()
    {
        if (movingCount == Number.Multiple) return;

        if (INV_ItemSlot.CanHandleObjects(moveObjectCode, 1))
        {
            INV_ItemSlot.SendObject(moveObjectCode, 1);
            S_ObjectControls.DestroyObject(moveObject);
        }
        else
            return;
        //add logic for adding object
        EndMoveState();
    }

    public void MoveToOriginal()
    {
        if (movingType == Type.ExistingObject)
        {
            Move(originalPivot);
            EndMovePlace();
            return;
        }

        TakeObject();

    }
}
