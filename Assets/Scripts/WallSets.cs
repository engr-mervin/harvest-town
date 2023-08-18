using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSets : MonoBehaviour
{
    public GameObject[] wallSets;

    public int currentActive;

    public void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        for(int i = 0;i<wallSets.Length;i++)
        {
            if (wallSets[i] == null) continue;

            if (i == currentActive)
                wallSets[i].SetActive(true);
            else
                wallSets[i].SetActive(false);
        }
    }
}
