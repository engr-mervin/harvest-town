using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BC_ActionOption
{
    
    private Animator animator;
    private BoxCollider2D box;
    private bool isOpen;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        animator.Play("Def1");
        isOpen = false;
    }


    public void Open()
    {
        animator.Play("Open1");
        isOpen = true;
        
        foreach(Vector2Int block in GetComponent<ObjectTransform>().AllBlockPoints())
        {
            AStar_Grid.GetNode(block).walkable = true;
        }

        box.enabled = false;
    }

    public void Close()
    {
        animator.Play("Close1");
        isOpen = false;

        foreach (Vector2Int block in GetComponent<ObjectTransform>().AllBlockPoints())
        {
            AStar_Grid.GetNode(block).walkable = false;
        }

        box.enabled = true;
    }

    public override void ButtonClicked()
    {
        if (!isOpen)
            Open();
        else
            Close();

        GM.playerState.SetState(new MovementState(GM.playerState));
    }
}


