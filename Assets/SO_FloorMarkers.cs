using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_FloorMarkers : MonoBehaviour
{
    public static SO_FloorMarkers instance;
    private void Awake()
    {
        instance = this;
    }
}
