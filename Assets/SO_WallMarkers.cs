using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_WallMarkers : MonoBehaviour
{
    public static SO_WallMarkers instance;
    private void Awake()
    {
        instance = this;
    }
}
