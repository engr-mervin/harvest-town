using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class Buttons_MainMenuSlots : MonoBehaviour
{
    public GameObject[] enable;
    public GameObject slotInfo;
    public Image screenCapture;
    public Text slotDetails1,slotDetails2;
    public GameObject[] disable;
    public int currentSlot;
    public Button[] slots;
    public Color highlightColor;
    public Color defaultColor;
    public Sprite[] screenShots;

    public void Awake()
    {
        screenShots = new Sprite[4];
        for (int i =0;i<slots.Length;i++)
        {
            string path = Application.persistentDataPath + "/player." + i.ToString();
            if (!File.Exists(path))
                slots[i].interactable = false;

        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            string path = Application.persistentDataPath + "/player." + i.ToString();
            if (!File.Exists(path))
                slots[i].interactable = false;

        }
        SlotClick(0);
    }
    private void OnDisable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<Image>().color = defaultColor;
        }
    }
    public void Back()
    {
        MyFunctions.Toggle(enable, disable);
    }
    
    public void SlotClick(int slot)
    {
        if (currentSlot == slot)
            return;

        currentSlot = slot;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == slot)
                slots[i].GetComponent<Image>().color = highlightColor;
            else
                slots[i].GetComponent<Image>().color = defaultColor;
        }
        slotInfo.SetActive(true);
        SetDisplay(slot);
        SetDetails(slot);
    }
    void SetDisplay(int i)
    {
        if(screenShots[i] !=null)
        {
            screenCapture.sprite = screenShots[i];
            return;
        }

        Texture2D tex = new Texture2D(Screen.width, Screen.height);

        if (!File.Exists(Application.persistentDataPath + "/player." + i.ToString() + "_disp.png"))
        {
            screenCapture.sprite = null;
            screenShots[i] = null;
            return;
        }
            
        byte[] allBytes = File.ReadAllBytes(Application.persistentDataPath + "/player." + i.ToString() + "_disp.png");
        tex.LoadImage(allBytes);
        
        screenCapture.sprite = Sprite.Create(tex, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        screenShots[i] = screenCapture.sprite;
    }
    void SetDetails(int i)
    {
        string path = Application.persistentDataPath + "/player." + i.ToString() ;

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Open);

        SaveFile load = formatter.Deserialize(stream) as SaveFile;

        stream.Close();

        slotDetails1.text = "Name: " + load.playerName + Environment.NewLine + "Money: " + load.playerMoney.ToString();
        slotDetails2.text = "Date: " + load.currDate + Environment.NewLine + "Time: " + load.currTime;
    }
    public void LoadSlot()
    {
        string path = Application.persistentDataPath + "/player." + currentSlot.ToString();

        if (File.Exists(path))
            LoadingScreen.instance.LoadScene(2,currentSlot);
        else
            Debug.Log("No save data exists");
    }
}
