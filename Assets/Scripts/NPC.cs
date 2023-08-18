using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string greetings
    {
        get { return GetGreetings(); }
    }

    private static string playerName;

    public List<string> greetingType;
    private void Awake()
    {
        CreatePlayer.OnPlayerCreation += SetName;
    }
    private void SetName()
    {
        playerName = GM.player.playerName;
        greetingType = new List<string>()
        {
            "Hello " + playerName +"!",
            "Good Morning " +playerName+".",
            "What can I do for you today?",
        };
    }

    public string GetGreetings()
    {
        return greetingType[1];//**** random
    }
}
