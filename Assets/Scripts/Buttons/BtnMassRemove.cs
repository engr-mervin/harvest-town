
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BtnMassRemove : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;
    public BtnSelectMode bsm;
    public BtnUndoTile but;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (tc.selectMarker == null || tc.selectMarker.Count == 0) return;
        bool b = true;

        foreach(STR_GridObject m in tc.selectMarker)
        {
            if (S_Tilemap.walls.GetTile<Tile>(m.pos) == null && S_Tilemap.floors.GetTile<Tile>(m.pos) == null) continue;
                
            Tilemap tm;
            if (S_Tilemap.walls.GetTile<Tile>(m.pos) != null) //case 1  there is a wall
                tm = S_Tilemap.walls;
            else  //case 2 there is a floor
                tm = S_Tilemap.floors;

            STR_UndoTile ut;
            if (b)
            {
                ut = new STR_UndoTile(tm.GetTile<Tile>(m.pos), m.pos, tm, STR_UndoTile.Type.StartMultiple);
                b = false;
            }
            else
            {
                ut = new STR_UndoTile(tm.GetTile<Tile>(m.pos), m.pos, tm, STR_UndoTile.Type.Multiple);
            }

            tc.undoTiles.Add(ut);
            print((tc.undoTiles.Count - 1) + ":" + tc.undoTiles[tc.undoTiles.Count - 1].type);

            S_Tilemap.walls.SetTile(m.pos, null);
            S_Tilemap.floors.SetTile(m.pos, null);
        }
        if(tc.undoTiles.Count!=0)
        but.GetComponent<Button>().interactable = true;
    }

}
