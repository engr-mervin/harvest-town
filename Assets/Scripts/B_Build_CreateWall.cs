using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_Build_CreateWall : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;

    public Vector2Int min;
    public Vector2Int max;

    public int count;

    public void OnPointerClick(PointerEventData eventData)
    {
        S_CreateWall s = new S_CreateWall(ref tc.wallsMarker);

        min = s.min;
        max = s.max;

        count = s.check.Count;

        tc.walls.Clear();
        foreach(STR_GridObject gm in tc.wallsMarker)
        {
            tc.walls.Add(gm.pos);
        }

    }
}
