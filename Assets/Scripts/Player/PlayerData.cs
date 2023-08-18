using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string playerType;
    public int money;
    public string playerName;
    private PlayerMoney playerMoney;

    public int totalSteps;

    public float[] playerPosition = new float[3];

    public int[] lookDir = new int[2];
    public void Awake()
    {
        playerMoney = GetComponent<PlayerMoney>();
    }

    private void Update()
    {
        playerName = gameObject.name;
        totalSteps = GetComponent<Stats>().totalSteps;
        playerPosition[0] = transform.position.x;
        playerPosition[1] = transform.position.y;
        playerPosition[2] = transform.position.z;

        if(playerMoney!=null)
            money = playerMoney.money;

        lookDir[0] = GetComponent<PlayerMovement>().lookDir.x;
        lookDir[1] = GetComponent<PlayerMovement>().lookDir.y;

    }
}
