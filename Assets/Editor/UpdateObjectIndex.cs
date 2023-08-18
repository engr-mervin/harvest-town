using UnityEditor;
using UnityEngine;
public class UpdateObjectIndex : Editor
{
    [MenuItem("Custom/Update Object Indices")]
    public static void UpdateObjectIndices()
    {
        GameLoader loader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        OBJ_ObjectMaker objMaker = loader.objectLoader;
        for (int i =0;i< objMaker.objectDatabase.Length;i++)
        {
            if (objMaker.objectDatabase[i] == null) continue;
            SerializedObject so = new SerializedObject(objMaker.objectDatabase[i].GetComponent<OBJ_ObjectSaveData>());
            SerializedProperty index = so.FindProperty("objectIndex");
            index.intValue = i;
            so.ApplyModifiedProperties();
        }
    }
    [MenuItem("Custom/Compute Object Heights")]
    public static void ComputeObjectHeights()
    {
        GameLoader loader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        OBJ_ObjectMaker objMaker = loader.objectLoader;
        for (int i = 0; i < objMaker.objectDatabase.Length; i++)
        {
            if (objMaker.objectDatabase[i] == null) continue;
            SerializedObject so = new SerializedObject(objMaker.objectDatabase[i].GetComponent<ObjectTransform>());
            SerializedProperty height = so.FindProperty("height");

            ObjectTransform ot = objMaker.objectDatabase[i].GetComponent<ObjectTransform>();

            if (ot.top == null) continue;
            float computed = ot.top.position.y - ot.transform.position.y;

            height.floatValue = computed;
            so.ApplyModifiedProperties();
        }
    }

    [MenuItem("Custom/Update Wallset Indices")]
    public static void UpdateWallsetIndices()
    {
        GameLoader loader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        ITEM_WallpaperApplier wallMaker = loader.wallpaperLoader;
        for (int i = 0; i < wallMaker.wallsetsDatabase.Length; i++)
        {
            if (wallMaker.wallsetsDatabase[i] == null) continue; 
            
            SerializedObject so = new SerializedObject(wallMaker.wallsetsDatabase[i]);
            SerializedProperty index = so.FindProperty("index");
            index.intValue = i;
            so.ApplyModifiedProperties();
        }
    }


    [MenuItem("Custom/Update Carpet Indices")]
    public static void UpdateCarpetIndices()
    {
        GameLoader loader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        ITEM_CarpetApplier carpetMaker = loader.carpetLoader;

        for (int i = 0; i < carpetMaker.carpetsDatabase.Length; i++)
        {
            if (carpetMaker.carpetsDatabase[i] == null) continue;

            SerializedObject so = new SerializedObject(carpetMaker.carpetsDatabase[i]);
            SerializedProperty index = so.FindProperty("index");
            index.intValue = i;
            so.ApplyModifiedProperties();
        }
    }


    [MenuItem("Custom/Generate Item Codes")]

    public static void GenerateItemCodes()
    {
        UpdateObjectIndices();
        UpdateWallsetIndices();
        UpdateCarpetIndices();

        GameLoader loader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        ITEM_Database itemGenerator = loader.itemIndexer;

        for (int i = 0; i < itemGenerator.rawDatabase.Length; i++)
        {
            if (itemGenerator.rawDatabase[i].GetComponent<ITEM_Wallpaper>()!=null) //CASE WALLPAPER
            {
                SerializedObject so = new SerializedObject(itemGenerator.rawDatabase[i]);
                SerializedProperty index = so.FindProperty("itemCode");
                index.stringValue = "WP-" + itemGenerator.rawDatabase[i].GetComponent<ITEM_Wallpaper>().wallset.index.ToString();
                so.ApplyModifiedProperties();
            }

            if (itemGenerator.rawDatabase[i].GetComponent<ITEM_Carpets>() != null) //CASE CARPET
            {
                SerializedObject so = new SerializedObject(itemGenerator.rawDatabase[i]);
                SerializedProperty index = so.FindProperty("itemCode");
                index.stringValue = "CP-" + itemGenerator.rawDatabase[i].GetComponent<ITEM_Carpets>().carpet.index.ToString();
                so.ApplyModifiedProperties();
            }

            if (itemGenerator.rawDatabase[i].GetComponent<ITEM_Furnitures>() != null) //CASE FURNITURES
            {
                SerializedObject so = new SerializedObject(itemGenerator.rawDatabase[i]);
                SerializedProperty index = so.FindProperty("itemCode");
                index.stringValue = "OB-" + itemGenerator.rawDatabase[i].GetComponent<ITEM_Furnitures>().furniture.GetComponent<OBJ_ObjectSaveData>().objectIndex.ToString();
                so.ApplyModifiedProperties();
            }
        }
        itemGenerator.itemDatabase.Clear();
        for (int i = 0; i < itemGenerator.rawDatabase.Length; i++)
        {
            ITEM_Database.ItemData abc = new ITEM_Database.ItemData();
            abc.itemCode = itemGenerator.rawDatabase[i].itemCode;
            abc.item = itemGenerator.rawDatabase[i];
            itemGenerator.itemDatabase.Add(abc);

            ITEM_Furnitures fur = itemGenerator.rawDatabase[i].GetComponent<ITEM_Furnitures>();
            if (fur == null) continue;
            SerializedObject furniture = new SerializedObject(fur.furniture.GetComponent<OBJ_ObjectSaveData>());
            SerializedProperty indexofitem = furniture.FindProperty("itemIndex");
            indexofitem.stringValue = itemGenerator.rawDatabase[i].itemCode;
            furniture.ApplyModifiedProperties();
        }
    }

    [MenuItem("Custom/Update Wall Numbers")]
    public static void UpdateWallNumbers()
    {
        GameLoader gameLoader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        
        for (int i = 0; i < gameLoader.walls.Length; i++)
        {
            if (gameLoader.walls[i] == null) continue;

            SerializedObject so = new SerializedObject(gameLoader.walls[i].GetComponent<WallSaver>());
            SerializedProperty index = so.FindProperty("wallIndex");
            index.intValue = i;
            so.ApplyModifiedProperties();
        }
    }

}