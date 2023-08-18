using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionWander : MotionMoveTo
{
    
   
    [SerializeField]
    private float waitSec;
    [SerializeField]
    private float distance;
    [SerializeField]
    private bool compWander;
   
    

    [SerializeField]
    private float Range;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        compWander = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            targetPos = new Vector2(target.position.x, target.position.y);
        }

        distance = Vector2.Distance(rb.transform.position, targetPos);
        if (distance > chaseRadius)
        {
            if (!compWander)
                StartCoroutine("Wander");
        }
        else
        {
            StopCoroutine("Wander");
            compWander = false;
        }

    }

    private IEnumerator Wander()
    {
        Vector2 randomMovement;

        randomMovement = (new Vector2(Random.Range(-60, 61), Random.Range(-60, 61))*Range/60.0f) + rb.position;
        compWander = true;
        StartCoroutine(Go(randomMovement, 0.50f));

        yield return new WaitForSeconds(waitSec);

        compWander = false;
    }

    public IEnumerator Go(Vector2 target, float vicinity)
    {
        while (Vector2.Distance(rb.position, target) > vicinity)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        if (Vector2.Distance(rb.position, target) <= vicinity)
        {
            yield break;
        }

    }

}
