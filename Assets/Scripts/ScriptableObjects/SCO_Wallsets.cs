using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Wallset", menuName = "Wallset")]
public class SCO_Wallsets : ScriptableObject
{
    public int index;

    public Sprite L1, M1, R1, L2, M2, R2, L3, M3, R3, M4;
}
