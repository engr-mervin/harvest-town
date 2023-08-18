using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    public Slider progress;
    public static LoadingScreen instance;
    public Text progressText;
    public AsyncOperation operation;
    public Text loadingText;
    Coroutine loading;
    public GameObject canvas;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

    }

    
    public void LoadScene(int index,int slot)
    {
        canvas.SetActive(true);
        operation = null;
        StartCoroutine(LoadSceneAsync(index,slot));
        loading = StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        int i = 0;
        while (true)
        {
            if (i % 4 == 0)
                loadingText.text = "Loading";
            else if (i % 4 == 1)
                loadingText.text = "Loading.";
            else if (i % 4 == 2)
                loadingText.text = "Loading..";
            else if (i % 4 == 3)
                loadingText.text = "Loading...";

            i++;

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    IEnumerator LoadSceneAsync(int index,int slot)
    {
        operation = SceneManager.LoadSceneAsync(index);

        while(!operation.isDone)
        {
            float actualProgress = operation.progress / 0.9f;
            progress.value = actualProgress;
            SetProgressText(actualProgress);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(loading);

        if(slot<=3)
            new LoadGame("player." + slot.ToString());

        canvas.SetActive(false);
        yield break;
    }

    void SetProgressText(float actual)
    {
        progressText.text = (actual*100f).ToString("F2")+"%";
    }
}
