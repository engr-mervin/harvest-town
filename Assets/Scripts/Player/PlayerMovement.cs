using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //DOUBLE TAP CONTROL LIKE MINECRAFT START

    [SerializeField]
    private bool delayWaitStarted = false;
    private Coroutine delayWait;
    [SerializeField]
    private float doubleTapTimer;

    //DOUBLE TAP CONTROL LIKE MINECRAFT END

    //PLAYER MOVEMENT ANIMATION CONTROLS START
    [SerializeField]
    private enum MovementState
    {
        Stopped,
        Idling,
        Moving,
        Sitting
    }
    private MovementState moveState;

    [SerializeField]
    private int idleTransition;

    private bool idleStarted;

    [SerializeField]
    private Animator body, eyes, outfit, hair;

    //PLAYER MOVEMENT ANIMATION CONTROLS END

    public TC_Movement tC_Movement;


    public MC_MovingObject movingObject;
    private Rigidbody2D rb;



    public delegate void Step();
    public event Step OnStep;

    public event Step OnLook;
    public event Step OnStepVertical;
    public event Step OnHalfStep;

    public static event Step OnMove;

    public Vector2 actualMovement;
    //restraints
    public bool canMove;
    private bool canLook;
    public bool canPhone;

    private bool moveToIsFinished = false;
    public bool movingCoroutine = false;
    public bool MoveFinished
    {
    get { return moveToIsFinished; }
    }
    public bool hasVer;
    public bool hasHor;

    public Vector2Int pivotPosition;
    public Vector2Int blockPosition;

    public float moveHor;
    public float moveVer;
    public int lookHor;
    public int lookVer;

    public Vector2Int lookDir
    { get { return new Vector2Int(lookHor, lookVer); }
        set {
            lookHor = value.x;
            lookVer = value.y;
            }
    }
    public Vector2Int lastLook;

    public float walkSpeed;
    public float runSpeed;

    [SerializeField]
    private bool startMove=false;

    public enum State
    {
        Movement,
        ObjectMoving
    }
    [SerializeField]
    public State state = State.Movement; //enum for DPad State (Movement or Moving)


    public int runState;

    public Vector2 movementDirection;

    private Vector2Int lastGridPos;

    public Vector2Int lastPosition;

    public Vector2Int lastBlockPosition;

    void Awake()
    {
        pivotPosition = MyFunctions.TransformtoVector2Int(transform.position);
        rb = GetComponent<Rigidbody2D>();
        lastGridPos = pivotPosition;
        lastPosition = pivotPosition;
        lastBlockPosition = blockPosition;
        SetBlockPosition();
        //OnStep += PathFindingStep;
    }

    public void SetWalkable()
    {
        //AStar_Grid.GetNode(pivotPosition).walkable = false;
    }


    private void Update()
    {
        SetPivotPosition();
        SetBlockPosition();

        MoveStateUpdate();

        if (lastGridPos != pivotPosition)
        {
            OnStep?.Invoke();
            print("STEPPED");
        }
        if (lastGridPos.y != pivotPosition.y)
        {
            OnStepVertical?.Invoke();
        }
        if (lastBlockPosition != blockPosition)
        {
            OnHalfStep?.Invoke();
        }
        if (lastLook != lookDir)
        {
            OnLook?.Invoke();
        }
        lastPosition = lastGridPos;
        lastGridPos = pivotPosition;
        lastLook = lookDir;
        lastBlockPosition = blockPosition;
    }
    void Start()
    {
        lookVer = -1; //starting animation (look down)
        lookHor = 0;
        canMove = true;
        canPhone = true;
        canLook = true;
    }

    void FixedUpdate()
    {
        movementDirection = new Vector2(moveHor, moveVer);
        //NOT MOVING

        //MOVING
        if (startMove == false) return;

        actualMovement = movementDirection.normalized * Time.fixedDeltaTime;

        Vector2 movementFinal = walkSpeed * actualMovement;

        if (runState == 1)
        {
            movementFinal = runSpeed * actualMovement;
        }
        if (!canMove) return;
        rb.MovePosition(rb.position + movementFinal);
    }

    private void SetAnimatorParameters()
    {
        Animator[] animators = new Animator[4]
        {
            body,
            eyes,
            outfit,
            hair
        };

        foreach(Animator animator in animators)
        {
            if (animator == null) continue;
            animator.SetFloat("animHor", actualMovement.x);
            animator.SetFloat("animVer", actualMovement.y);
            animator.SetFloat("animSpeed", actualMovement.magnitude);

            animator.SetFloat("animLookHor", lookHor);
            animator.SetFloat("animLookVer", lookVer);

            animator.SetBool("isIdling", moveState == MovementState.Idling);
            animator.SetBool("isWalking", moveState == MovementState.Moving);
        }
    }
    private void MoveStateUpdate()
    {
        if (actualMovement.magnitude == 0f&&moveState!=MovementState.Idling)//STOPPED BUT NOT IDLING THEN START IDLE COUNTER
        {
            moveState = MovementState.Stopped;
            if (!idleStarted)
                StartCoroutine("Idle");
        }
        else if(actualMovement.magnitude != 0f)//MOVING
        {
            moveState = MovementState.Moving;
            if (idleStarted)
            {
                StopCoroutine("Idle");
                idleStarted = false;
            }
        }


        SetAnimatorParameters();
    }

    public IEnumerator Idle()
    {
        idleStarted = true;

        int i = 0;
        while (i < idleTransition && moveState==MovementState.Stopped)
        {
            i++;
            yield return new WaitForSeconds(0.1f);
        }

        if (i == idleTransition)
        {
            moveState = MovementState.Idling;
            yield break;
        }
    }

    public bool SamePosition(Vector2 position,float tolerance)
    {
        Vector2 currentPosition = (Vector2)(transform.position);
        Vector2 difference = position - currentPosition;

        if(difference.magnitude <= tolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public IEnumerator MoveToPosition(List<AStar_Node> nodes)
    {
        if (movingCoroutine|| nodes.Count == 0) //only one instance of this at a time
            yield break;

        movingCoroutine = true;
        moveToIsFinished = false;

        float time = 0.0f;

        for(int i =0;i<nodes.Count;i++)
        {
            float expectedTime;

            if (i > 0)
                expectedTime = 1.500f * ((nodes[i].position - nodes[i - 1].position).magnitude / walkSpeed);
            else
                expectedTime = 1.00f;

            if (expectedTime < 1.00f)
                expectedTime = 1.00f;

            Vector2 actualStep;
            bool goToNext;
            Vector2 currentTarget = (Vector2)(Positions.BlockToTransformCenter(nodes[i].position));
            do
            {
                float dirX = currentTarget.x - transform.position.x; //direction of movement
                float dirY = currentTarget.y - transform.position.y;

                movementDirection = new Vector2(dirX, dirY);
                
                if(movementDirection.normalized * Time.fixedDeltaTime!=actualMovement)
                    actualMovement = movementDirection.normalized * Time.fixedDeltaTime;

                actualStep = walkSpeed * actualMovement;

                if((currentTarget-rb.position).magnitude<actualStep.magnitude)
                {
                    actualStep = currentTarget - rb.position;
                }
                rb.MovePosition(rb.position + actualStep);

                SetLook(actualStep);
                time += Time.fixedDeltaTime;


                if (time > expectedTime)
                {
                    print("Took more than 1.5times the expected time to move to a new node, coroutine broken");
                    moveToIsFinished = false;
                    movingCoroutine = false;
                    StopMovement();
                    yield break;
                }

                //Check if can move to next node
                if (SamePosition(currentTarget, 0.005f))
                {
                    goToNext = true;
                }
                else
                {
                    goToNext = false;
                }
                yield return new WaitForFixedUpdate();
            }
            while (!goToNext);
            //Successfully finished a node, restart timer
            time = 0.0f;
        }
        //successfully finished the whole path
        moveToIsFinished = true;
        movingCoroutine = false;
        StopMovement();
        yield break;
    }

    void SetLook(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //X direction
            lookVer = 0;
            lookHor = (direction.x >= 0) ? 1 : -1;
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            //Y direction
            lookHor = 0;
            lookVer = (direction.y >= 0) ? 1 : -1;
        }
        else
            return;
    }
    public void SetPivotPosition()
    {
        pivotPosition = MyFunctions.TransformtoVector2Int(transform.position);
    }

    public void SetBlockPosition()
    {
        blockPosition = Positions.TransformToBlock(transform.position);
    }

    public void StopMovement()
    {
        actualMovement = Vector2.zero;
        moveHor = 0.0f;
        moveVer = 0.0f;
        hasHor = false;
        hasVer = false;
        startMove = false;

    }

    public void OverrideLook(Chair.DirectionType constraint)
    {
        switch(constraint)
        {
            case Chair.DirectionType.up:
            {
                lookVer = 1;
                return;
            }
            case Chair.DirectionType.down:
            {
                lookVer = -1;
                return;
            }
            case Chair.DirectionType.left:
            {
                lookHor = -1;
                return;
            }
            case Chair.DirectionType.right:
            {
                lookHor = 1;
                return;
            }
        }
    }
    public void EnableLook()
    {
        canLook = true;
    }
    public void DisableLook()
    {
        canLook = false;
    }



    public void MovePlayer(Vector2Int direction)
    {
        if(direction.x!=0)
        {
            if (!hasHor)
            {
                moveHor = direction.x;
            }
            if (canLook)
            {
                lookVer = 0;
                lookHor = direction.x;
            }
            hasHor = true;
        }
        if (direction.y!= 0)
        {
            if (!hasVer)
            {
                moveVer = direction.y;
            }
            if (canLook)
            {
                lookVer = direction.y;
                lookHor = 0;
            }
            hasVer = true;
        }
    }

    public void ExitPrimary(Vector2Int direction)
    {
        if (direction.x != 0)
        {
            moveHor = 0.0f;
            hasHor = false;
        }
        if (direction.y != 0)
        {
            moveVer = 0.0f;
            hasVer = false;
        }
    }

    public void SetObjectMoving(MC_MovingObject _movingObject)
    {
        movingObject = _movingObject;
        state = State.ObjectMoving;
    }
    public void SetMovement()
    {
        state = State.Movement;
    }

    public void ButtonMove(Vector2Int dir)
    {
        GM.playerMove.StartMovement(); //movement invoked

        if (!delayWaitStarted) //first tap
        {
            delayWait = StartCoroutine("DelayWaitForTap");
            runState = 0;
        }
        else //multiple taps
        {
            StopCoroutine(delayWait);
            delayWait = StartCoroutine("DelayWaitForTap");
            runState = 1;
        }


        GM.playerMove.MovePlayer(dir);
    }

    public void StartMovement()
    {
        Camera.main.GetComponent<FN_FollowPlayer>().Follow();
        OnMove?.Invoke();
        startMove = true;

        movingCoroutine = false; //touch movement
        StopAllCoroutines(); //stop moving to position coroutine only
        tC_Movement.ClearCommands();
    }

    IEnumerator DelayWaitForTap() //MINECRAFT LIKE CONTROL - 2 TAPS = RUN 1 TAP = WALK
    {
        delayWaitStarted = true;
        yield return new WaitForSeconds(doubleTapTimer);
        delayWaitStarted = false;
        yield break;
    }
}
