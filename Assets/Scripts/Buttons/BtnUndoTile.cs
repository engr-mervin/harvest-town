
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BtnUndoTile : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;
    public Button redo;
    public void OnPointerClick(PointerEventData eventData)
    {

        if (tc.undoTiles.Count == 0) return;
        if(tc.undoTiles[tc.undoTiles.Count - 1].type == STR_UndoTile.Type.Single)
        {
            Undo();
        }
        else if (tc.undoTiles[tc.undoTiles.Count - 1].type == STR_UndoTile.Type.Multiple)
        {
            int i = tc.undoTiles.Count - 1;
            bool a = true;

            while (tc.undoTiles[i].type!=STR_UndoTile.Type.StartMultiple)
            {
                if (a)
                {
                    Undo(STR_UndoTile.Type.StartMultiple); //save first redo as start
                    a = false;
                }
                else
                {
                    Undo(STR_UndoTile.Type.Multiple);
                }
                i--;
                print((tc.redoTiles.Count - 1) + ":" + tc.redoTiles[tc.redoTiles.Count - 1].type);
            }
            if (tc.undoTiles[i].type==STR_UndoTile.Type.StartMultiple)
            {
                Undo(STR_UndoTile.Type.Multiple);
            }
        }
    }

    public void Undo(STR_UndoTile.Type type = STR_UndoTile.Type.Single)
    {
        STR_UndoTile ut = tc.undoTiles[tc.undoTiles.Count - 1];

        Tilemap tm;
        if (S_Tilemap.walls.GetTile<Tile>(ut.pos) != null) //case 1  there is a wall
            tm = S_Tilemap.walls;
        else if (S_Tilemap.floors.GetTile<Tile>(ut.pos) != null) //case 2 there is a floor
            tm = S_Tilemap.floors;
        else//case 3 there is nothing
            tm = ut.tilemap;

        STR_UndoTile rt = new STR_UndoTile(tm.GetTile<Tile>(ut.pos), ut.pos, tm,type);

        tc.redoTiles.Add(rt);

        ut.tilemap.SetTile(ut.pos, ut.tile);

        tc.undoTiles.Remove(ut);

        if (tm != ut.tilemap)
            tm.SetTile(ut.pos, null);

        if (tc.undoTiles.Count == 0)
            this.GetComponent<Button>().interactable = false;

        if (redo.interactable == false)
            redo.interactable = true;
    }
}
