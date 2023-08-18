using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvTileSlot : MonoBehaviour,IPointerClickHandler
{
    public Tile tile;
    public TC_Build tc;
    public GameObject marker;
    public BtnUndoTile but;
    public BtnSelectMode bsm;

    public static InvTileSlot active;

    public void Awake()
    {
        if (marker.GetComponent<RectTransform>().localPosition == GetComponent<RectTransform>().localPosition)
            active = this;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Refresh();
        marker.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition;
        active = this;
        MassApply();
    }
    public void Refresh()
    {
        tc.active = tile;
        if (tile != null)
            tc.activeTileMap = S_Tilemap.GetTilemapFromName(tile.name);
        else
            tc.activeTileMap = null;
    }

    public void MassApply()
    {
        if (tc.selectMarker == null || tc.selectMarker.Count == 0 || tile==null) return;
        bool b = true;

        foreach (STR_GridObject m in tc.selectMarker)
        {
            Tilemap tm;
            if (S_Tilemap.walls.GetTile<Tile>(m.pos) != null) //case 1  there is a wall
                tm = S_Tilemap.walls;
            else if (S_Tilemap.floors.GetTile<Tile>(m.pos) != null) //case 2 there is a floor
                tm = S_Tilemap.floors;
            else
                tm = tc.activeTileMap;

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

            if (tc.activeTileMap == S_Tilemap.walls)
            {
                S_Tilemap.walls.SetTile(m.pos, tile);
                S_Tilemap.floors.SetTile(m.pos, null);
            }
            else
            {
                S_Tilemap.floors.SetTile(m.pos, tile);
                S_Tilemap.walls.SetTile(m.pos, null);
            }
        }
        tc.redoTiles.Clear();
        tc.redo.interactable = false;
        but.GetComponent<Button>().interactable = true;
    }
}
