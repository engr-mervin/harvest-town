using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_PlayerWalls : MonoBehaviour //S is singleton
{
    public static SO_PlayerWalls instance;
    private void Awake()
    {
        instance = this;
    }


    public static List<STR_Walls> FindContinuousWall(Vector3Int pos)
    {
        List<STR_Walls> continuous = new List<STR_Walls>();
        //add left

        Vector3Int startLeft = pos + Vector3Int.left;
        while(WallsManager.wallList.Find(c=>c.pos==startLeft)!=null)
        {
            continuous.Add(WallsManager.wallList.Find(c => c.pos == startLeft));
            startLeft += Vector3Int.left;
        }
        //add right

        Vector3Int startRight = pos + Vector3Int.right;
        while (WallsManager.wallList.Find(c => c.pos == startRight) != null)
        {
            continuous.Add(WallsManager.wallList.Find(c => c.pos == startRight));
            startRight += Vector3Int.right;
        }

        return continuous;
    }
}
