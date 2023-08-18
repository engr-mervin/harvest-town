using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class STR_Floors
{
    public Vector3Int pos;
    public SCO_Carpets carpet;
    public GameObject marker;

    public STR_Floors(Vector3Int _pos, SCO_Carpets _carpet, GameObject _marker)
    {
        pos = _pos;
        carpet = _carpet;
        marker = _marker;

        S_Tilemap.floors.SetTile(pos, carpet.tile);

        marker.transform.position = pos + new Vector3(0.50f, 0.50f, 0f);
        marker.transform.parent = SO_FloorMarkers.instance.transform;

        FloorsManager.floorList.Add(this);
    }

    public void ChangeTile(SCO_Carpets _carpet)
    {
        Debug.Log("Changed tile");
        carpet = _carpet;

        S_Tilemap.floors.SetTile(pos, carpet.tile);
    }
}
