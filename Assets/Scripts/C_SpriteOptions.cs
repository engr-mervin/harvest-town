using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class C_SpriteOptions// C is for construct
{
    public string spriteName;
    public Sprite sprite;
}

[System.Serializable]
public class C_CustomizableSprite
{
    public string spriteID;

    public SpriteRenderer sprite;

    public C_SpriteOptions[] spriteOptions;
}
