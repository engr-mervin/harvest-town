using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Vector2 animationVector;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animationVector = rb.velocity.normalized;

        animator.SetFloat("Speed", animationVector.sqrMagnitude);

        if (animationVector.magnitude <= 0.05f) return;
        SetAnimation(animationVector);
        SetLookDirection(animationVector);

    }

    public void SetAnimation(Vector2 animationVector)
    {
        animator.SetFloat("Horizontal", animationVector.x);
        animator.SetFloat("Vertical", animationVector.y);
    }
    public void SetLookDirection(Vector2 animationVector)
    { 
        if (Mathf.Abs(animationVector.x) >= Mathf.Abs(animationVector.y))
        {
            if (animationVector.x < 0)
                animator.SetFloat("LookDir", 3f); //left
            else
                animator.SetFloat("LookDir", 4f); //right
        }
        else
        {
            if (animationVector.y < 0)
                animator.SetFloat("LookDir", 1f); //down
            else
                animator.SetFloat("LookDir", 2f); //up
        }
    }
}
