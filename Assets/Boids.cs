using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boids : MonoBehaviour,IPointerClickHandler
{
    public List<BoidAgent> allBoids = new List<BoidAgent>();

    public GameObject boid;

    [Header("Parameters")]

    public int numberOfBoids;
    public int maxNumberOfBoids;

    public float minSpeed;
    public float maxSpeed;

    public bool canCohere, canSeparate, canAlign;
    public float cohesion, separation, separationPower, alignment;

    [Header("Radii parameters")]
    public float searchRadius;
    public float separationRadius;
    [Tooltip("Distance for avoiding edges of screen")]
    public float avoidEdgeRadius;


    [Header("Others")]

    [Tooltip("Dot product for getting front boids")]
    public float dot;

    [Tooltip("Min distance for separation")]
    public float minDist;

    public float maxX, minX, maxY, minY;
    [Tooltip("Whether get front boids or boids less than a certain distance")]
    public bool atFront;

    private void Awake()
    {
        allBoids.Clear();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < numberOfBoids; i++)
        {
            GameObject g = Instantiate(boid, this.transform);

            g.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-390f, 390f), Random.Range(-190f, 190f), 0f);
            BoidAgent ba = g.GetComponent<BoidAgent>();
            allBoids.Add(ba);
            ba.Initialize();
            yield return new WaitForEndOfFrame();

        }

        yield break;
    }
    public void Update()
    {
        //GetAllNeighbors();
        if (atFront)
            GetAllFront();
        else
            GetAllNeighbors();

        foreach(BoidAgent b in allBoids)
        {
            //add boid calculations
            Cohere(b);
            Separate(b);
            Align(b);

            //Check if it will collide with wall the next frame, if yes add a vector in the opposite direction
            //b.CheckPosition();

            AvoidEdges(b);

            b.VelocityClamp();
            //angle
            b.Angle();
            //move
            b.rt.localPosition += (Vector3)b.velocity * Time.deltaTime;
            b.disp = b.velocity.magnitude;
        }
    }
    public void GetAllNeighbors()
    {
        if (allBoids.Count == 0)
            return;
        foreach (BoidAgent b in allBoids)
        {
            b.nearby.Clear();
        }
        for (int i = 0; i < allBoids.Count; i++)
        {
            BoidAgent current = allBoids[i];

            for (int j = i + 1; j < allBoids.Count; j++)
            {
                BoidAgent check = allBoids[j];
                float dist = Vector3.Distance(current.rt.localPosition, check.rt.localPosition);

                if (dist <= searchRadius)
                {
                    current.nearby.Add(check);
                    check.nearby.Add(current);
                }
            }
        }
    }

    public bool IsInside(Vector3 pos)
    {
        if (pos.x < maxX && pos.x > minX && pos.y > minY && pos.y < maxY)
            return true;
        else
            return false;
    }
    private void AvoidEdges(BoidAgent b)
    {
        Vector3 pos = b.rt.localPosition;

        Vector3 sug = pos + (Vector3)b.velocity.normalized * avoidEdgeRadius;

        if (IsInside(sug))
            return;

        Quaternion q = b.GetAngle();

        float A = (q.eulerAngles.z - 45f)*Mathf.PI/180f;
        float B = (q.eulerAngles.z - 30f)*Mathf.PI / 180f;
        float C = (q.eulerAngles.z -15f)*Mathf.PI / 180f;
        float D = (q.eulerAngles.z)*Mathf.PI / 180f;
        float E = (q.eulerAngles.z + 15f)*Mathf.PI / 180f;

        float F = (q.eulerAngles.z + 30f) * Mathf.PI / 180f;
        float G = (q.eulerAngles.z + 45f) * Mathf.PI / 180f;
        //right and left
        Vector2 AA = new Vector2(Mathf.Cos(A), Mathf.Sin(A));
        Vector2 BB = new Vector2(Mathf.Cos(B), Mathf.Sin(B));
        Vector2 CC = new Vector2(Mathf.Cos(C), Mathf.Sin(C));
        Vector2 DD = new Vector2(Mathf.Cos(D), Mathf.Sin(D));
        Vector2 EE = new Vector2(Mathf.Cos(E), Mathf.Sin(E));

        Vector2 FF = new Vector2(Mathf.Cos(F), Mathf.Sin(F));
        Vector2 GG = new Vector2(Mathf.Cos(G), Mathf.Sin(G));

        Vector2[] all =
        {
            AA,
            BB,
            CC,
            DD,
            EE,
            FF,
            GG
        };
        Vector2 bias = -b.velocity;

        foreach(Vector2 vc in all)
        {
            Vector3 ext = pos + (Vector3)vc.normalized * avoidEdgeRadius;
            if (IsInside(ext))
            {
                bias = vc;
                break;
            }
        }

        b.velocity = bias;

    }
    public void GetAllFront()
    {
        if (allBoids.Count == 0)
            return;
        foreach (BoidAgent b in allBoids)
        {
            b.nearby.Clear();
        }
        for (int i = 0; i < allBoids.Count; i++)
        {
            BoidAgent a = allBoids[i];

            foreach (BoidAgent b in allBoids)
            {
                if(b==a)
                    continue;

                Vector2 dist = b.rt.localPosition - a.rt.localPosition;

                if (dist.magnitude > searchRadius)
                    continue;

                if(Vector2.Dot(a.velocity,dist)<=dot)
                {
                    continue;
                }

                a.nearby.Add(b);

            }
        }
    }

    public void Cohere(BoidAgent b)
    {
        if (b.nearby.Count == 0||!canCohere)
            return;

        Vector3 center = Vector3.zero;

        foreach (BoidAgent a in b.nearby)
        {
            center += a.rt.localPosition;
        }
        center /= b.nearby.Count;

        Vector3 diff = center - b.rt.localPosition;

        b.velocity += (Vector2)diff * cohesion;

    }

    public void Separate(BoidAgent b)
    {
        if (b.nearby.Count == 0||!canSeparate)
            return;

        Vector3 sum = Vector3.zero;
        
        foreach (BoidAgent a in b.nearby)
        {
            float dist = Vector3.Distance(b.rt.localPosition, a.rt.localPosition);

            if (dist <= minDist)
                dist = minDist;
            if (dist > separationRadius)
                continue;
            //(1 / (Mathf.Pow(dist/radiusOfAvoidance, separationPower)))
            //(radiusOfAvoidance-dist)
            Vector3 curr = (Mathf.Pow(separationRadius / dist, separationPower)) * (b.rt.localPosition - a.rt.localPosition);

            sum += curr;
        }
        Vector3 diff = sum/b.nearby.Count;

        b.velocity += (Vector2)diff * separation;
    }

    public void Align(BoidAgent b)
    {

        if (b.nearby.Count == 0||!canAlign)
            return;

        Vector2 sum = Vector2.zero;

        foreach (BoidAgent a in b.nearby)
        {
            sum += a.velocity;
        }

        sum /= b.nearby.Count;

        b.velocity += sum * alignment;
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (allBoids.Count >= maxNumberOfBoids)
            return;

        GameObject g = Instantiate(boid, this.transform);
        print(eventData.position);
        g.GetComponent<RectTransform>().localPosition = MyFunctions.ScreenPositionToRectTransform(eventData.position); 
        BoidAgent ba = g.GetComponent<BoidAgent>();
        allBoids.Add(ba);
        ba.Initialize();
    }
    
}
