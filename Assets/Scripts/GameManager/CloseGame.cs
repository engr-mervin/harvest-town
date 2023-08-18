using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame
{
    public delegate void Close();
    public static event Close OnCloseGame;
    public CloseGame()
    {
        GameObject.FindObjectOfType<GOD_Initialization>().ClearLists();
        
        //delete current objects
        S_WorldBlocks.DestroyAllObjects();
        //delete Player
        DeletePlayer();
        //delete tiles
        S_Tilemap.walls.ClearAllTiles();
        S_Tilemap.floors.ClearAllTiles();

        StatePanel.Terminate();

        OnCloseGame?.Invoke();

    }
    private void DeletePlayer()
    {
        if (GM.playerObj == null)
            return;
        UnityEngine.GameObject.Destroy(GM.playerObj);
    }
}
