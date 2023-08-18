using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefData : MonoBehaviour
{
    public float cameraHeight;

    public void Refresh()
    {
        cameraHeight = Camera.main.orthographicSize;
    }
}
