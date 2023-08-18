
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BtnSelectMode : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;
    public Sprite selectMode;
    public Sprite applyMode;

    public GameObject[] toggle;


    void Awake()
    {
        if (!tc.selectMode)
        {
            GetComponent<Image>().sprite = applyMode;
        }
        else
        {
            GetComponent<Image>().sprite = selectMode;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (tc.selectMode)
        {
            Unselect();   
        }
        else
        {
            tc.selectMode = true;
            GetComponent<Image>().sprite = selectMode;

            MyFunctions.ToggleObjects(toggle);

        }
    }

    public void Unselect()
    {
        foreach (STR_GridObject gm in tc.selectMarker)
        {
            Destroy(gm.g);
        }
        tc.selectMarker.Clear();
        tc.selectCol.Clear();
        tc.selectUncol.Clear();

        tc.selectMode = false;
        GetComponent<Image>().sprite = applyMode;

        MyFunctions.ToggleObjects(toggle);

    }
}
