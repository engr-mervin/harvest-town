using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer
{
    private GameObject create;
    public delegate void PlayerCreation();
    public static event PlayerCreation OnPlayerCreation;

    public CreatePlayer(string playerType,string playerName)//Create an object
    {
        //Create Player
        create = (GameObject)GameObject.Instantiate(Resources.Load("Player"));
        
        //Sets animations
        create.GetComponent<Player>().character = Resources.Load("Characters/"+playerType) as Character;

        //for future saving
        create.GetComponent<Player>().playerName = playerName;
        create.GetComponent<Player>().playerType = playerType;

        create.GetComponent<Player>().Initialize();

        //Sets GM variables-to be modified//optional
        SetGMVariables(create);

        GM.playerState.SetState(new MovementState(GM.playerState));

    }

    public GameObject GetPlayer()
    {
        return create;
    }

    private void SetGMVariables(GameObject g)
    {
        GM.playerObj = g;
        GM.player = g.GetComponent<Player>();
        GM.playerAnim = g.GetComponent<MovementAnimation>();
        GM.playerMove = g.GetComponent<PlayerMovement>();
        GM.playerState = g.GetComponent<PlayerStateManager>();
        GM.playerMoney = g.GetComponent<PlayerMoney>();


        OnPlayerCreation?.Invoke();
    }
}
