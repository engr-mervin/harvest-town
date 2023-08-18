using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPosRenderer : MonoBehaviour
{

    private Renderer myRenderer;
    public float offset;
    public float yPos;
    public bool runOnlyOnce;
    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
        offset = gameObject.GetComponent<BoxCollider2D>().offset.y;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        yPos = (gameObject.GetComponent<Transform>().position.y + offset)*10;
        myRenderer.sortingOrder = (int)(5000f - yPos);

        if(runOnlyOnce)
        {
            Destroy(this);
        }

      
        
    }
}
