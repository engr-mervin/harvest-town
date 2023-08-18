using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAgent : MonoBehaviour
{
    public Vector2 velocity;

    public float disp;

    public RectTransform rt;
    public Boids boid;
    public List<BoidAgent> nearby = new List<BoidAgent>();

    public void Initialize()
    {
        boid = GetComponentInParent<Boids>();
        rt = GetComponent<RectTransform>();
        //set default speed
        velocity = (new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f))).normalized * boid.minSpeed;
        //set angle
        Angle();
    }
    public void VelocityClamp()
    {
        if(velocity.magnitude>boid.maxSpeed)
            velocity = velocity.normalized * boid.maxSpeed;

        if(velocity.magnitude<boid.minSpeed)
            velocity = velocity.normalized * boid.minSpeed;
    }
    public void Angle()
    {
        float a;
        if (velocity.x != 0)
            a = Mathf.Atan(velocity.y / velocity.x) * 180 / Mathf.PI;
        else
            a = 0f;

        if(velocity.x<0f)
        {
            a += 180f;
        }

        Quaternion b = new Quaternion();
        b.eulerAngles = new Vector3(0f, 0f, a - 45f);

        transform.rotation = b;
    }
    public Quaternion GetAngle()
    {
        float a;
        if (velocity.x != 0)
            a = Mathf.Atan(velocity.y / velocity.x) * 180f / Mathf.PI;
        else
            a = 0f;

        if (velocity.x < 0f)
        {
            a += 180f;
        }

        Quaternion b = new Quaternion();
        b.eulerAngles = new Vector3(0f, 0f, a);

        return b;
    }
    public void CheckPosition()
    {
        Vector3 nextPos = rt.localPosition + (Vector3)velocity * Time.deltaTime;

        if (nextPos.x <= 395f && nextPos.x >= -395f && nextPos.y <= 195f && nextPos.y >= -195f)
            return;

        if (nextPos.x < -395f)
            rt.localPosition = new Vector3(395f, rt.localPosition.y);
        if (nextPos.x > 395f)
            rt.localPosition = new Vector3(-395f, rt.localPosition.y);
        if (nextPos.y > 195f)
            rt.localPosition = new Vector3(rt.localPosition.x, -195f);
        if (nextPos.y < -195f)
            rt.localPosition = new Vector3(rt.localPosition.x, 195f);

    }
}
