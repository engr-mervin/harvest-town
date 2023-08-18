using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{

    public GameObject move;
    public GameObject interact;
    public bool isHeld = false;
    private Coroutine heldMove;
    void Awake()
    {
        CreatePlayer.OnPlayerCreation += Subscribe;
        CloseGame.OnCloseGame += Unsubscribe;
    }
    

    private void Unsubscribe()
    {
        CreatePlayer.OnPlayerCreation -= Subscribe;
        CloseGame.OnCloseGame -= Unsubscribe;
        GM.playerMove.OnHalfStep -= Check;
        GM.playerMove.OnLook -= Check;
    }

    void Subscribe()
    {
        GM.playerMove.OnHalfStep += Check;
        GM.playerMove.OnLook += Check;
    }

    void Check()
    {
        GameObject g = InteractRayCast.GetNearestObject();
        GameObject h = InteractRayCast.GetNearestInteractable();


        if (g != null&& g.GetComponent<Placeable>()!=null)
            move.GetComponent<Button>().interactable = true;
        else
            move.GetComponent<Button>().interactable = false;

        if (h != null && h.GetComponents<BC_ActionOption>().Length != 0)
            interact.GetComponent<Button>().interactable = true;
        else
            interact.GetComponent<Button>().interactable = false;
    }


    //BUTTON METHODS

    //DOWN
    public void HoldMove()
    {
        if (isHeld == false)
        {
            isHeld = true;
            heldMove = StartCoroutine(HoldMoveCoroutine());
        }
    }
    IEnumerator HoldMoveCoroutine()
    {
        float timer = 0.0f;
        while(isHeld==true&&timer<1.0f)
        {
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        GameObject movable = InteractRayCast.GetLowestObject();
        print(movable.gameObject);
        bool a = false;
        ObjectTransform objectTrans = movable.GetComponent<ObjectTransform>();
        foreach(Vector2Int block in objectTrans.AllBlockPoints())
        {
            Block b = S_WorldBlocks.GetBlockinPosition(block);
            if (b.topMostObject == movable)
                continue;
            else
            {
                a = true;
                break;
            }
        }
        if(a)
        {
            GM.playerState.SetState(new MovingObjectState(GM.playerState,movable, MovingObject.Type.ExistingObject, MovingObject.Number.Multiple));
            yield break;
        }
        else
        {
            GM.playerState.SetState(new MovingObjectState(GM.playerState,movable, MovingObject.Type.ExistingObject));
            yield break;
        }
    }

    //UP/EXIT
    public void ReleaseMove()
    {
        if (isHeld == true)
        {
            isHeld = false;
            if (heldMove != null)
                StopCoroutine(heldMove);
        }
    }
    public void Move()
    {
        GameObject g = InteractRayCast.GetNearestObject();

        if (g != null && g.GetComponent<Placeable>() != null)
            InteractRayCast.GetNearestObject().GetComponent<Placeable>().ButtonClicked();

    }

    public void Interact()
    {
        GameObject g = InteractRayCast.GetNearestInteractable();

        //ONLY ONE OPTION
        if (g != null && g.GetComponent<BC_ActionOption>() != null && g.GetComponents<BC_ActionOption>().Length == 1)
            InteractRayCast.GetNearestInteractable().GetComponent<BC_ActionOption>().ButtonClicked();
        else
            GM.playerState.SetState(new InteractingState(GM.playerState,GM.playerMove)) ;
    }
}
