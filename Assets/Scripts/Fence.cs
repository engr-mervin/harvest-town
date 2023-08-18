using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject[] gs;

    public bool isDestroyed = false;
    public ObjectTransform objectTrans;
    public void Awake()
    {
        gs = new GameObject[]
            {
                up,
                down,
                left,
                right
            };

        GetComponent<ObjectSorter>().onObjectMoved += RefreshAll;
        objectTrans = GetComponent<ObjectTransform>();

        RefreshAll();
    }
    public static Vector2Int[] dirs = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private List<Vector2Int> refreshFence = new List<Vector2Int>();

    public void RefreshThisFence()
    {
        if (isDestroyed == true) return;

        for (int i = 0; i < 4; i++)
        {
            Vector2Int currentpos = objectTrans.pivot + dirs[i];

            GameObject current = STR_GridObjectWithLayer.list.Find(a => a.gridPosition == currentpos)?.blockObject;

            if (current != null && current.GetComponent<Fence>() != null && current.GetComponent<Fence>().isDestroyed == false) //convert to another component fenceconnectable//create gameobjects where walls are
            {
                GameObject corr=null;
                Fence corrf = current.GetComponent<Fence>();
                switch (i)
                {
                    case 0:
                        corr = corrf.down;
                        break;
                    case 1:
                        corr = corrf.up;
                        break;
                    case 2:
                        corr = corrf.right;
                        break;
                    case 3:
                        corr = corrf.left;
                        break;

                }
                if(corr.activeSelf == false)
                    gs[i].SetActive(true);
            }

            else
            {
                gs[i].SetActive(false);
            }
        }
    }

    private void PopulateRefreshList()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2Int currentpos = objectTrans.pivot + dirs[i];
            Vector2Int lastpos = MyFunctions.TransformtoVector2Int(GetComponent<ObjectSorter>().lastlastPosition) + dirs[i];

            if (currentpos == MyFunctions.TransformtoVector2Int(GetComponent<ObjectSorter>().lastlastPosition))
            {
                refreshFence.Add(lastpos);
                continue;
            }
            else if (lastpos == objectTrans.pivot)
            {
                refreshFence.Add(currentpos);
                continue;
            }
            else
            {
                refreshFence.Add(currentpos);
                refreshFence.Add(lastpos);
            }
        }
    }
    public void RefreshAll() //call when placing or removing //add then refresh //remove then refresh
    {
        RefreshThisFence();

        PopulateRefreshList();

        foreach (Vector2Int v in refreshFence)
        {
            GameObject current = STR_GridObjectWithLayer.list.Find(a => a.gridPosition == v)?.blockObject;
            if (current != null && current.GetComponent<Fence>() != null)
            {
                current.GetComponent<Fence>().RefreshThisFence();
            }
        }
    }

    public void DeleteRefresh()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2Int currentpos = objectTrans.pivot + dirs[i];

            GameObject current = STR_GridObjectWithLayer.list.Find(a => a.gridPosition == currentpos)?.blockObject;

            if(current != null && current.GetComponent<Fence>() != null)
            {
                current.GetComponent<Fence>().RefreshThisFence();
            }
        }
    }
    private void OnDestroy()
    {
        isDestroyed = true;
        DeleteRefresh();
    }
}
