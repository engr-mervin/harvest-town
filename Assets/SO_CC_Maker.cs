using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SO_CC_Maker : MonoBehaviour
{
    public string resourceName;
    public string playerName;

    public static SO_CC_Maker instance;
    private void Awake()
    {
        instance = this;
    }
}
