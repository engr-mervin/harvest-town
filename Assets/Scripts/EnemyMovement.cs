using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    public float moveSpeedProp
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    [SerializeField]
    private Vector2 animationVector;
    [SerializeField]
    private Vector2 randomMovement;

    private Rigidbody2D rb;
    private Enemy enemy;
    private EnemyAnimation enemyAnimation;

    [SerializeField]
    private bool compWander;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }
   
    public void Go(Vector2 target) // go to a location or target
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
    }

    

    public void Stop()
    {
        animationVector = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

    public IEnumerator Wander()
    {
        if (!compWander)
        {
            randomMovement = new Vector2(Random.Range(-10, 11) * 0.10f, Random.Range(-10, 11) * 0.10f) + rb.position;
            compWander = true;


            yield return new WaitForSeconds(enemy.wanderWait);

            compWander = false;
        }

        yield return new WaitForSeconds(enemy.wanderWait);
        if (compWander)
        {
            Go(randomMovement);
        }

    }

    public void Look(Vector2 target)
    {
        animationVector = Vector2.ClampMagnitude(target - rb.position, 1.0f);
        enemyAnimation.SetLookDirection(animationVector);
    }

    public void startMethodWander()
    {
        StartCoroutine("Wander");
    }
    public void stopMethodWander()
    {
        StopCoroutine("Wander");
        compWander = false;
    }
}