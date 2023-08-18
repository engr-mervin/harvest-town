using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{

    public Vector3 sittingOffset;

    public LayerMask placeableOn;

    bool someoneIsSitting = false;

    Vector3 lastStandingPos;

    public bool hasAnim;


    public enum DirectionType
    { 
        right,
        left,
        up,
        down,
        all
    }
    ;

    public DirectionType directionType;

    public void Interact(GameObject interactor)
    {
        MovementAnimation sitterMovementAnim = interactor.GetComponent<MovementAnimation>();

        if (!someoneIsSitting && !sitterMovementAnim.isSitting)
        {
            lastStandingPos = interactor.transform.position;
            Sit(interactor);
        }
        else if (sitterMovementAnim.isSitting)
        {
            UnSit(interactor);
            interactor.transform.position = lastStandingPos;
        }

    }

    void UnSit(GameObject st)
    {
        MovementAnimation stAnim = st.GetComponent<MovementAnimation>();
        EdgeCollider2D stCollider = st.GetComponent<EdgeCollider2D>();
        PlayerMovement stMove = st.GetComponent<PlayerMovement>();

        if (directionType != DirectionType.all)
            {
                stMove.EnableLook();
            }
            stAnim.isSitting = false;

            stMove.canMove = true;
            stCollider.enabled = true;
            someoneIsSitting = false;
    }


    void Sit(GameObject st)
    {

        MovementAnimation stAnim = st.GetComponent<MovementAnimation>();
        EdgeCollider2D stCollider = st.GetComponent<EdgeCollider2D>();
        SpriteRenderer stRender = st.GetComponent<SpriteRenderer>();
        PlayerMovement stMove = st.GetComponent<PlayerMovement>();
       
        if (directionType != DirectionType.all)
        {
            stMove.DisableLook();
            stMove.OverrideLook(directionType);
        }
        stAnim.isSitting = true;

        st.transform.position = gameObject.transform.position + sittingOffset;
        stMove.canMove = false;
        stCollider.enabled = false;
        stRender.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        someoneIsSitting = true;
       
    }

    public bool LayerMaskContainsLayer(int layer)
    {
        return MyFunctions.LayerMaskContainsLayer(placeableOn, layer);
    }
}
