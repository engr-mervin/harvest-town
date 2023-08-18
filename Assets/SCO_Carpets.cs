using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(fileName = "New Carpet",menuName = "Carpet")]
public class SCO_Carpets : ScriptableObject
{
    public int index;
    public Tile tile;
}
