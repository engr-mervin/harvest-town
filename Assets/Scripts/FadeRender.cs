using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRender : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Transform objectToFade;
    public Vector2 offsetBL;
    public Vector2 offsetTR;
    public float fadeAlpha;

    public Transform player;

    void Start()
    {
        fadeAlpha = Uprefs.objectFadeAlpha;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        objectToFade = gameObject.GetComponent<Transform>();
    }
    void Update()
    {
        if (player == null) return;
        Vector2 BL = new Vector2(objectToFade.position.x + offsetBL.x, objectToFade.position.y + offsetBL.y);
        Vector2 TR = new Vector2(objectToFade.position.x + offsetTR.x, objectToFade.position.y + offsetTR.y);
        Vector4 fadecolor = new Color();
        fadecolor.w = sprite.color.r;
        fadecolor.x = sprite.color.g;
        fadecolor.y = sprite.color.b;

        if (player.position.x >= BL.x && player.position.x <= TR.x && player.position.y >= BL.y && player.position.y <= TR.y) 
        {

            fadecolor.z = fadeAlpha;
        } 
        else
        {
            fadecolor.z = 1.00f;
        }
        sprite.color = fadecolor;

    }
}
