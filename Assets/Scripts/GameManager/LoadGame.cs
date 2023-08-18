
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadGame
{
    public delegate void PlayerCreation();
    public static event PlayerCreation OnPlayerCreation;
    public LoadGame(string filename)
    {
        //LOAD FILE
        SaveFile load = LoadFile(filename);

        if (load == null) return;

        //CREATE NEW GAME
        NewGame ng = new NewGame(load.playerType, load.playerName,NewGame.Type.Load);

        //BUILD PLAYER TILES - FOR MODIFICATION - WILL SPAWN TWICE ON CREATE FLOORS
        BuildTiles(load);

        //CREATE WALLS
        WallsManager.CreateWalls(load, GameGod.gameLoader);

        //CREATE FLOORS
        FloorsManager.CreateFloors(load, GameGod.gameLoader);

        //SET PLAYER DATA
        TurnOverData(ng.player, load);

        OnPlayerCreation?.Invoke();

        //SET PLAYER MONEY
        GM.playerMoney.LoadGame(load);

        //INITIALIZE PATHFINDING
        Vector2Int bl = new Vector2Int(-500, -500);
        Vector2Int tr = new Vector2Int(500, 500);
        AStar_Grid.instance.Initialize(bl, tr);


        //SET NPC SPAWN AS UNWALKABLE
        foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
        {
            pm.SetWalkable();
        }
        //INSTANTIATE PLAYER OBJECTS
        InstantiatePlayerObjects(load,GameGod.gameLoader.objectLoader);

        foreach(INV_ItemSlot itemslot in INV_ItemSlot.slots)
        {
            itemslot.Awake();
        }

        //LOAD PLAYER ITEMS

        LoadItems(load);
    }

    public void LoadItems(SaveFile saveFile)
    {
        for(int i = 0; i<INV_ItemSlot.slots.Length;i++)
        {
            if (saveFile.itemCode[i] == "") continue;

            INV_ItemSlot.slots[i].SetItem(saveFile.itemCode[i], saveFile.itemQty[i]);
        }
    }
    public SaveFile LoadFile(string filename)
    {
        string path = Application.persistentDataPath + "/"+filename;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SaveFile load = formatter.Deserialize(stream) as SaveFile;

            stream.Close();

            return load;
        }
        else
        {
            Debug.LogError("No Save Data");
            return null;
        }
    }

    private void BuildTiles(SaveFile sd)
    {
        GameObject.FindObjectOfType<S_Tilemap>().Awake();

        for (int i = 0; i < sd.tilePosX2.Length; i++) //uncollideable tilemap
        {
            //Get current position
            Vector3Int pos = new Vector3Int(sd.tilePosX2[i], sd.tilePosY2[i], 0);

            //Get tile from resources
            string s = sd.tileName2[i].Substring(0, 1);
            Tile tile = Resources.Load("PaletteTiles/" + s + "/" +sd.tileName2[i],typeof(Tile)) as Tile;

            //Apply tile
            S_Tilemap.floors.SetTile(pos, tile);
        }
        for (int i = 0; i < sd.tilePosX1.Length; i++) //collideable tilemap
        {
            //Get current position
            Vector3Int pos = new Vector3Int(sd.tilePosX1[i], sd.tilePosY1[i], 0);

            //Get tile from resources
            string s = sd.tileName1[i].Substring(0, 1);
            Tile tile = Resources.Load("PaletteTiles/" + s + "/" + sd.tileName1[i], typeof(Tile)) as Tile;

            //Apply tile
            S_Tilemap.walls.SetTile(pos, tile);
        }
    }


    private void TurnOverData(GameObject player, SaveFile saveData)
    {
        //location of player
        player.transform.position = new Vector3(saveData.position[0], saveData.position[1], saveData.position[2]);
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.lookDir = new Vector2Int(saveData.playerLookDir[0], saveData.playerLookDir[1]);

        //Stats class - for future expansion
        Stats stats = player.GetComponent<Stats>();
        stats.totalSteps = saveData.steps;
    }

    private void InstantiatePlayerObjects(SaveFile savedFile,OBJ_ObjectMaker maker)
    {
        for (int j = 0; j <= 3; j++)
        {
            for (int i = 0; i < savedFile.objectCount; i++)
            {
                if (savedFile.objectLayer[i] == j)
                {
                    Vector2Int pivot = new Vector2Int(savedFile.objectPivotX[i], savedFile.objectPivotY[i]);

                    GameObject currentObject = GameObject.Instantiate(maker.objectDatabase[savedFile.objectIndex[i]]);
                    currentObject.transform.parent = S_ObjectControls.ObjectInstantiator.transform;

                    if (currentObject == null)
                    {
                        Debug.Log("No gameobject found with index of " + savedFile.objectIndex[i]);
                        continue;
                    }
                    else
                    {
                        MovingObject mo = StatePanel.movingObject.GetComponent<MovingObject>();
                        mo.StartMoving( currentObject,GM.playerMove, MovingObject.Type.NewObject);
                        
                        if (mo.rotatable != null)
                            mo.rotatable.Rotate(savedFile.objectRotateIndex[i]);

                        mo.Move(pivot);
                        mo.EndMovePlace();
                    }
                }
            }
        }
    }
}
