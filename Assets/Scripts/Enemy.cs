using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{  //Base Stats
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private float chaseRadius;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private bool isHoming;
    [SerializeField]
    private bool isWandering;
    

    //used by movement
    public float wanderWait;


    //behavior parameters
    [SerializeField]
    private float distanceToPlayer;
    [SerializeField]
    private bool fromChasing;
    [SerializeField]
    private bool isHome;
    [SerializeField]
    private Vector2 targetPos;
    [SerializeField]
    private Vector2 home;

    //properties
   
    public float maxHealthProp
    {
        get { return maxHealth; }
    }
    public Vector2 targetPosProp
    {
        get { return targetPos; }
    }


    //components
    private Transform target;
    private Rigidbody2D enemyRB;
    private EnemyMovement enemyMovement;
    private EnemyCombat enemyCombat;
 
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyRB = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<EnemyCombat>();
    }
    void Start()
    {
        home = enemyRB.position;
        fromChasing = false;
    }
    void FixedUpdate() 
    {
        if (target != null)
        {
            targetPos = new Vector2(target.position.x, target.position.y);
        }

        distanceToPlayer = Vector2.Distance(enemyRB.position, targetPos);
        //Behavior
        if (target != null)
        {
            if (distanceToPlayer <= chaseRadius) //player in vicinity
            {
                enemyMovement.stopMethodWander();
                if (distanceToPlayer > attackRadius)  //chase
                {
                    fromChasing = true;
                    enemyMovement.Go(targetPos);
                   
                }
                else if (distanceToPlayer <= attackRadius)  //attack
                {
                    if (fromChasing)
                    {
                        fromChasing = false;
                    }
                    enemyMovement.Stop();
                    enemyMovement.Look(targetPos);
                    enemyCombat.methodStartAttack();
                }

            }
            else if (distanceToPlayer > chaseRadius) //player out of sight
            {
                if (isHoming && !isHome)
                {
                    enemyMovement.Go(home);
                }
                else if (isHoming & isHome)
                {
                    enemyMovement.Stop();
                }
                else if (!isHoming && isWandering) //wander
                {
                    if (fromChasing)
                    {
                        enemyMovement.Stop();
                        fromChasing = false;
                    }
                    enemyMovement.startMethodWander();
                }
                else if (!isHoming && !isWandering)
                {
                        enemyMovement.Stop();
                        enemyMovement.Look(targetPos);
                }
            }
        }

    }
    void Update()
    {
        

        if(enemyRB.position == home)
        {
            isHome = true;
        }
        else
        {
            isHome = false;
        }

    }

    //custom methods
    




}

