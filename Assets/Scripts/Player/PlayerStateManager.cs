using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager :MonoBehaviour
{
    public PlayerState currentState;
    public GameObject currentPanel;

    public void SetState(PlayerState playerState)
    {
        currentState = playerState;
    }

    public void HidePanel()
    {
        currentPanel.SetActive(false);
        HideHotBar();
        Time.timeScale = 1.00f;
    }

    public void ShowPanel()
    {
        print("asdfasddsf2");
        currentPanel.SetActive(true);
        HotBar(currentState);
        SetTimeScale(currentState);
    }

    public IEnumerator ScreenShot()
    {
        HidePanel();
        if (Application.platform == RuntimePlatform.Android)
        {
            ScreenCapture.CaptureScreenshot("lastPicture.png");
        }
        else
        {
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath+"/lastPicture.png");
        }

        yield return new WaitForSecondsRealtime(0.50f);

        ShowPanel();

        yield break;
    }

    public void ScreenShotMethod()
    {
        StartCoroutine(ScreenShot());
    }

    public void HotBar(PlayerState ps)
    {

        if (ps.GetType() == typeof(MovementState))
            ShowHotBar();

        if (ps.GetType() == typeof(MovingObjectState))
            HideHotBar();

        if (ps.GetType() == typeof(AddingFloorsState))
            HideHotBar();

        if (ps.GetType() == typeof(AddingWallsState))
            HideHotBar();

        if (ps.GetType() == typeof(BuyingState))
            ShowHotBar();

        if (ps.GetType() == typeof(PauseState))
            HideHotBar();

        if (ps.GetType() == typeof(InteractingState))
            HideHotBar();

    }
    public void SetTimeScale(PlayerState ps)
    {
        if (ps.GetType() == typeof(MovementState))
            Time.timeScale = 1.0f;

        if (ps.GetType() == typeof(MovingObjectState))
            Time.timeScale = 1.0f;

        if (ps.GetType() == typeof(AddingFloorsState))
            Time.timeScale = 1.0f;

        if (ps.GetType() == typeof(AddingWallsState))
            Time.timeScale = 1.0f;

        if (ps.GetType() == typeof(BuyingState))
            Time.timeScale = 1.0f;

        if (ps.GetType() == typeof(PauseState))
            Time.timeScale = 0.0f;

        if (ps.GetType() == typeof(InteractingState))
            Time.timeScale = 0.0f;

    }
    internal void HideHotBar()
    {
        StatePanel.hotBar.SetActive(false);
    }
    internal void ShowHotBar()
    {
        StatePanel.hotBar.SetActive(true);
    }
}
