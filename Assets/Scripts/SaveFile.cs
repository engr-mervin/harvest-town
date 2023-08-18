using System;
[System.Serializable]
public class SaveFile
{
    //Player Data
    public string playerType;
    public string playerName;

    public float[] position;
    public int[] playerLookDir;
    public int steps;

    public int playerMoney;

    //preferences
    public float cameraHeight;

    //Objects Data
    public int objectCount;
    public int[] objectRotateIndex;
    public int[] objectPivotX;
    public int[] objectPivotY;
    public int[] objectIndex;
    public int[] objectLayer;
    public bool[] objectRCX;
    public bool[] objectRCY;

    //Tile Data
    public int[] tilePosX1;
    public int[] tilePosY1;
    public string[] tileName1;

    public int[] tilePosX2;
    public int[] tilePosY2;
    public string[] tileName2;

    //WallObject Data
    public int[] wallPosX;
    public int[] wallPosY;
    public int[] wallIndices;
    public int[] wallPaperIndices;

    //FloorObject Data
    public int[] floorPosX;
    public int[] floorPosY;
    public string[] floorPath;
    public int[] floorIndex;

    //Inventory Data
    public string[] itemCode;
    public int[] itemQty;

    //Time and Date
    public string currDate;
    public string currTime;

    public SaveFile(PlayerData playerData, OBJ_ObjectSaveData[] objectsData, SaveTileMap[] stms,PrefData preferences)
    {
        //TIME AND DATE
        currDate = DateTime.Now.ToString("MMM dd") + ", " + DateTime.Now.ToString("yyyy");
        currTime = DateTime.Now.ToString("HH")+":"+ DateTime.Now.ToString("mm");
        //PLAYER
        playerType = playerData.playerType;
        playerName = playerData.playerName;
        playerMoney = playerData.money;

        position = new float[3];
        position[0] = playerData.playerPosition[0];
        position[1] = playerData.playerPosition[1];
        position[2] = playerData.playerPosition[2];

        playerLookDir = new int[2];
        playerLookDir[0] = playerData.lookDir[0];
        playerLookDir[1] = playerData.lookDir[1];

        steps = playerData.totalSteps;

        //PREFERENCES
        cameraHeight = preferences.cameraHeight;

        //Objects
        SaveObjects(objectsData);
        //TILES

        tilePosX1 = new int[stms[0].tileData.Count];
        tilePosY1 = new int[stms[0].tileData.Count];
        tileName1 = new string[stms[0].tileData.Count];

        for (int i = 0; i < stms[0].tileData.Count; i++)
        {
            tilePosX1[i] = stms[0].tileData[i].x;
            tilePosY1[i] = stms[0].tileData[i].y;
            tileName1[i] = stms[0].tileData[i].name;
        }

        tilePosX2 = new int[stms[1].tileData.Count];
        tilePosY2 = new int[stms[1].tileData.Count];
        tileName2 = new string[stms[1].tileData.Count];

        for (int i = 0; i < stms[1].tileData.Count; i++)
        {
            tilePosX2[i] = stms[1].tileData[i].x;
            tilePosY2[i] = stms[1].tileData[i].y;
            tileName2[i] = stms[1].tileData[i].name;
        }



        //walls
        STR_Walls[] copy = WallsManager.wallList.ToArray();

        wallPosX = new int[copy.Length];
        wallPosY = new int[copy.Length];
        wallIndices = new int[copy.Length];
        wallPaperIndices= new int[copy.Length];

        for (int i =0;i<copy.Length;i++)
        {
            wallPosX[i] = copy[i].pos.x;
            wallPosY[i] = copy[i].pos.y;
            wallIndices[i] = copy[i].wallIndex;
            wallPaperIndices[i] = copy[i].wallPaperIndex;
        }

        //floors
        STR_Floors[] copyf = FloorsManager.floorList.ToArray();


        floorPosX = new int[copyf.Length];
        floorPosY = new int[copyf.Length];
        floorIndex = new int[copyf.Length];

        for (int i = 0; i < copyf.Length; i++)
        {
            floorPosX[i] = copyf[i].pos.x;
            floorPosY[i] = copyf[i].pos.y;
            floorIndex[i] = copyf[i].carpet.index;
        }

        //inventory

        INV_ItemSlot[] copyitem = INV_ItemSlot.slots;

        itemCode = new string[copyitem.Length];
        itemQty = new int[copyitem.Length];

        for (int i = 0; i < copyitem.Length; i++)
        {
            if (copyitem[i].item == null)
                itemCode[i] = "";
            else
                itemCode[i] = copyitem[i].item.itemCode;

            itemQty[i] = copyitem[i].quantity;
        }

    }

    private void SaveObjects(OBJ_ObjectSaveData[] objectsData)
    {
        //OBJECTS
        objectCount = objectsData.Length;
        objectPivotX = new int[objectCount];
        objectPivotY = new int[objectCount];
        objectIndex = new int[objectCount];
        objectLayer = new int[objectCount];
        objectRotateIndex = new int[objectCount];
        objectRCX = new bool[objectCount];
        objectRCY = new bool[objectCount];

        for (int i = 0; i < objectCount; i++)
        {
            objectPivotX[i] = objectsData[i].pivot.x;
            objectPivotY[i] = objectsData[i].pivot.y;
            objectIndex[i] = objectsData[i].objectIndex;
            objectLayer[i] = objectsData[i].layer;
            objectRotateIndex[i] = objectsData[i].rotateIndex;
            objectRCX[i] = objectsData[i].recenteredX;
            objectRCY[i] = objectsData[i].recenteredY;
        }

    }
}
