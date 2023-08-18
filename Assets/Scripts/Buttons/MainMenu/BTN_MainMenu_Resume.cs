
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_MainMenu_Resume : MonoBehaviour, IPointerClickHandler
{
    private void Awake()
    {
        string path = Application.persistentDataPath + "/player.0";
        if (!File.Exists(path))
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = true;

        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        string path = Application.persistentDataPath + "/player.0";

        if (File.Exists(path))
            GM.instance.LoadGame(0);
        else
            Debug.Log("No save data exists");
    }
}
