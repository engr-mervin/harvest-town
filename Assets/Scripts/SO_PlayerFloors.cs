using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_PlayerFloors : MonoBehaviour //S is singleton
{
    public static SO_PlayerFloors instance;
    private void Awake()
    {
        instance = this;
    }
}
