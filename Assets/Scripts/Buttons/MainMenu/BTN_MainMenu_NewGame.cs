using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class BTN_MainMenu_NewGame : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GM.instance.CharacterCreation();
    }
}