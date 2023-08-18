using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaver : MonoBehaviour
{
    private Coroutine autosave;
    public TopLeftNotification topLeftNotif;
    public void Awake()
    {
        autosave = StartCoroutine(RecursiveTimer());
    }

    IEnumerator RecursiveTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(Options.instance.autoSaveInterval*60f);
            while(GM.playerState.currentState.GetType() != typeof(MovementState))
            {
                yield return new WaitForSeconds(1.0f);
            }
            Autosave();
        }
    }


    private void Autosave()
    {
        new SaveGame("player.0");
        topLeftNotif.SetNotif("AUTOSAVING...");
    }
}
