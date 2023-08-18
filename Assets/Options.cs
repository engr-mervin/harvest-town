using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Options : MonoBehaviour
{
    public static Options instance;

    [Range(2000,20000)]
    public int pathfindingCalculations;
    [Range(5,30)]
    public int autoSaveInterval;
    [Range(5f, 10f)]
    public float viewHeight;

    public GameObject canvas;
    public GameObject[] tabs;
    public int currentTab;

    public Slider pathFindSlider;
    public Text pathFindVal;

    public Slider autosaveSlider;
    public Text autosaveVal;

    public Slider viewSlider;
    public Text viewVal;


    public OptionsFile current;
    public Button apply;

    public void OnCanvasEnable()
    {
        currentTab = 0;
        ChangeTab(0);

        
        GetCurrent();
        Set();
    }
    
    void Set()
    {
        pathFindSlider.value = current.pathFindCalculations / 100;
        autosaveSlider.value = current.autoSaveInterval / 5;
        viewSlider.value = current.viewHeight;

        pathFindVal.text = current.pathFindCalculations.ToString();
        autosaveVal.text = current.autoSaveInterval.ToString();
        viewVal.text = current.viewHeight.ToString("0.##");
    }    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        OnCanvasEnable();
    }

    void GetCurrent() //get saved values every time options is enabled
    {
        if (current == null)
            return;
        string path = Application.persistentDataPath + "/playerPrefs";

        BinaryFormatter formatter = new BinaryFormatter();

        if (!File.Exists(path))
        {
            current = new OptionsFile(this);
            return;
        }

        FileStream stream = new FileStream(path, FileMode.Open);

        current = formatter.Deserialize(stream) as OptionsFile;

        stream.Close();
    }

    public void ChangeTab(int tab)
    {
        for(int i =0;i<tabs.Length;i++)
        {
            if (i == tab)
                tabs[i].SetActive(true);
            else
                tabs[i].SetActive(false);
        }
    }

    public void Back()
    {
        canvas.SetActive(false);
    }

    public void PathfindSlider()
    {
        pathFindVal.text = (pathFindSlider.value * 100f).ToString();
        Check();
    }
    public void AutosaveSlider()
    {
        autosaveVal.text = (autosaveSlider.value * 5f).ToString();
        Check();
    }
    public void ViewSlider()
    {
        viewVal.text = (viewSlider.value).ToString("0.##");
        Check();
    }
    public void Check()
    {

        if (current.pathFindCalculations != (int)(pathFindSlider.value * 100f))
        {
            apply.interactable = true;
            return;
        }
        if (current.autoSaveInterval != (int)(autosaveSlider.value * 5f))
        {
            apply.interactable = true;
            return;
        }

        if (current.viewHeight != (viewSlider.value ))
        {
            apply.interactable = true;
            return;
        }

        apply.interactable = false;
    }

    public void ApplyChanges()
    {
        pathfindingCalculations = (int)(pathFindSlider.value*100f);
        autoSaveInterval = (int)(autosaveSlider.value * 5f);
        viewHeight = (viewSlider.value);
        UpdateViewHeight();
        SaveOptions();
        Check();
    }
    private void UpdateViewHeight()
    {
        if (Camera.main.GetComponent<FN_FollowPlayer>() == null)
            return;

        Camera.main.orthographicSize = viewHeight;
    }

    public void SaveOptions()
    {
        string path = Application.persistentDataPath + "/playerPrefs";

        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(path))
            File.Delete(path);
        
            FileStream stream = new FileStream(path, FileMode.Create);

            current = new OptionsFile(this);

            formatter.Serialize(stream, current);

            stream.Close();
        }
    }


