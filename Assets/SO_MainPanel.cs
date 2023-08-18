using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_MainPanel : MonoBehaviour
{
    public static SO_MainPanel instance;
    private void Awake()
    {
        instance = this;
    }

}
