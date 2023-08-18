using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coordinates : MonoBehaviour
{
    TMP_Text coord;
    private void Awake()
    {
        coord = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (GM.playerMove == null) return;

        coord.text = "x: " + GM.playerMove.pivotPosition.x + ", y: " + GM.playerMove.pivotPosition.y;
    }
}
