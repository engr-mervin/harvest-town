
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BtnRedoTile : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;
    public Button undo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (tc.redoTiles.Count == 0) return;
        if (tc.redoTiles[tc.redoTiles.Count - 1].type == STR_UndoTile.Type.Single)
        {
            Redo();
        }
        else if (tc.redoTiles[tc.redoTiles.Count - 1].type == STR_UndoTile.Type.Multiple)
        {
            int i = tc.redoTiles.Count - 1;
            bool a = true;

            while (tc.redoTiles[i].type != STR_UndoTile.Type.StartMultiple)
            {
                if (a)
                {
                    Redo(STR_UndoTile.Type.StartMultiple); //save first redo as start
                    a = false;
                }
                else
                {
                    Redo(STR_UndoTile.Type.Multiple);
                }
                i--;
            }
            if (tc.redoTiles[i].type == STR_UndoTile.Type.StartMultiple)
            {
                Redo(STR_UndoTile.Type.Multiple);
            }
        }
    }

    public void Redo(STR_UndoTile.Type type = STR_UndoTile.Type.Single)
    {
        if (tc.redoTiles.Count == 0) return;

        STR_UndoTile rt = tc.redoTiles[tc.redoTiles.Count - 1];

        Tilemap tm;

        if (S_Tilemap.walls.GetTile<Tile>(rt.pos) != null) //case 1  there is a wall
            tm = S_Tilemap.walls;
        else if (S_Tilemap.floors.GetTile<Tile>(rt.pos) != null) //case 2 there is a floor
            tm = S_Tilemap.floors;
        else//case 3 there is nothing
            tm = rt.tilemap;

        STR_UndoTile ut = new STR_UndoTile(tm.GetTile<Tile>(rt.pos), rt.pos, tm,type);

        tc.undoTiles.Add(ut);
        rt.tilemap.SetTile(rt.pos, rt.tile);

        tc.redoTiles.Remove(rt);


        if (tm != rt.tilemap)
            tm.SetTile(rt.pos, null);


        if (tc.redoTiles.Count == 0)
            this.GetComponent<Button>().interactable = false;

        if (undo.interactable == false)
            undo.interactable = true;
    }
}
