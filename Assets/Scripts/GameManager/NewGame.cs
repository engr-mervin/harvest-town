using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame
{
    public GameObject player;
    public enum Type
    {
        New,
        Load,
    }

    public NewGame(string resourceName, string playerName, Type type)
    {
        //UI SPAWNERS
        GameGod gameGod = GameObject.FindObjectOfType<GameGod>();
        gameGod.Initialize();

        //DRAW UIs
        DrawUI drawer = gameGod.gameObject.GetComponent<DrawUI>();
        drawer.DrawUIs();

        //TILE DATA MANAGER
        TileManager tileManager = GameObject.FindObjectOfType<TileManager>();
        tileManager.Initialize();
        
        //OBJECT INITIALIZERS
        GOD_Initialization gi = gameGod.gameObject.GetComponent<GOD_Initialization>();
        
            //INITIALIZE STATE PANELS
            gi.Initialize();

            //CREATE PLAYER - FOR MODIFICATION
            player = new CreatePlayer(resourceName, playerName).GetPlayer();
        
            //SET PLAYER MONEY
            GM.playerMoney.NewGame();
        
            //ATTACH TC MOVEMENT TO PLAYER
            gi.SetTCMovement();
        

        //CREATE NPCS
        new CreateNPCs();

        GameObject.FindObjectOfType<AStar_Grid>().SetInstance();

        if (type==Type.New)
        {
            //INITIALIZE PATHFINDING
            Vector2Int bl = new Vector2Int(-500, -500);
            Vector2Int tr = new Vector2Int(500, 500);
            AStar_Grid.instance.Initialize(bl, tr);

            //SET NPC SPAWN AS UNWALKABLE
            foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
            {
                pm.SetWalkable();
            }
        }

    }

}
