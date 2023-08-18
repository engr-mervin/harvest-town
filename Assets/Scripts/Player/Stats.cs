using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
[System.Serializable]
public class Stats : MonoBehaviour //these are the game stats of player
{
    [SerializeField]
    private string playerName;
    [SerializeField]
    public int totalSteps;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private Player player;


    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnStep += AddStep;

    }
    private void Start()
    {
        playerName = player.playerName;
        
    }

    private void AddStep()
    {
        totalSteps++;
    }

}
