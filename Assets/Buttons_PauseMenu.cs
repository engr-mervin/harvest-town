using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Runtime.Serialization.Formatters.Binary;


public class Buttons_PauseMenu : MonoBehaviour
{
    public GameObject[] def;
    public GameObject[] options;
    public GameObject[] slots;

    public GameObject confirmation;
    public Text confText;

    public GameObject completion;

    public int currentSlot;

    public enum Current
    {
        Default,
        Options,
        Slots
    }

    public Current currentWindow;

    public void Resume()
    {
        GM.playerState.SetState(new MovementState(GM.playerState));
    }
    public void YesConfirm()
    {
        confirmation.SetActive(false);

        GM.playerState.ScreenShotMethod();

        new SaveGame("player." + currentSlot.ToString(), true);

        completion.SetActive(true);
    }

    public void NoConfirm()
    {
        confirmation.SetActive(false);
    }
    public void OkComplete()
    {
        completion.SetActive(false);
        Default();
    }
    public void SaveSlot(int slot)
    {
        string path = Application.persistentDataPath + "/player." + slot.ToString();

        if (!File.Exists(path))
        {
            GM.playerState.ScreenShot();
            new SaveGame("player." + slot.ToString(),true);
            completion.SetActive(true);
        }
        else
        {
            currentSlot = slot;
            confirmation.SetActive(true);

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SaveFile load = formatter.Deserialize(stream) as SaveFile;

            stream.Close();

            confText.text = "OVERWRITE SAVED GAME ON SLOT " + slot.ToString() +"("+ load.playerName+", "+load.playerMoney+")? YOU CAN'T UNDO THIS ACTION!" ;
        }

    }



    private void OnEnable()
    {
        Default();
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;

        new SaveGame("player.0");

        new CloseGame();

        LoadingScreen.instance.LoadScene(0, 5);
    }
    public void Default()
    {
        MyFunctions.Toggle(def, options, slots);
        currentWindow = Current.Default;
    }
    public void OptionsButton()
    {
        Options.instance.canvas.SetActive(true);
        currentWindow = Current.Options;
    }

    public void Save()
    {
        MyFunctions.Toggle(slots, def, options);
        currentWindow = Current.Slots;
    }
    public void Back()
    {
        Default();
    }

    
}
