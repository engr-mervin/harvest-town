using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;
using UnityEngine.Networking;

public class SaveGame
{
    public SaveGame(string filename,bool lastPic=false)
    {
        if (GM.player == null) return;
        if(lastPic)
        {
            string picPath = Application.persistentDataPath + "/" + filename + "_disp.png";
            if (File.Exists(picPath))
            {
                File.Delete(picPath);
            }
            File.Copy(Application.persistentDataPath + "/lastPicture.png", picPath);
        }
        PlayerData playerData = GM.player.GetComponent<PlayerData>();

        List<SaveTileMap> stms = new List<SaveTileMap>();

        PrefData preferences = GameObject.FindObjectOfType<PrefData>();

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/"+filename;

        if (File.Exists(path))
        {
            

            File.Delete(path);

            FileStream stream = new FileStream(path, FileMode.Create);

            foreach(Tilemap tm in S_Tilemap.allTilemaps)
            {
                stms.Add(new SaveTileMap(tm));
            }


            preferences.Refresh();

            SaveFile save = new SaveFile(playerData, OBJ_ObjectSaveList.instance.objectsList.ToArray(), stms.ToArray(), preferences);

            formatter.Serialize(stream, save);

            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);

            foreach (Tilemap tm in S_Tilemap.allTilemaps)
            {
                stms.Add(new SaveTileMap(tm));
            }


            SaveFile save = new SaveFile(playerData, OBJ_ObjectSaveList.instance.objectsList.ToArray(), stms.ToArray(), preferences);

            formatter.Serialize(stream, save);

            stream.Close();
        }
    }
}
