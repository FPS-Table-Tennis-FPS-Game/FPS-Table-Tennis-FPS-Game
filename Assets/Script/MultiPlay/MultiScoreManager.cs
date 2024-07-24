using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;

    public GameObject[] setPosition;

    public GameObject[] players = new GameObject[2];

    private float waitTime = 1f;

    public string currrentTurn;

    public override void Spawned()
    {
        base.Spawned();
        networkRunner = FindObjectOfType<NetworkRunner>();
        setPosition = GameObject.FindGameObjectsWithTag("PositionSetting");
        StartCoroutine(FindPlayers(waitTime));
    }

    IEnumerator FindPlayers(float time)
    {
        yield return new WaitForSeconds(time);

        int index = 0;
        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            string playerKey = "player" + index;
            NetworkObject playerObject = Runner.GetPlayerObject(player);

            if (playerObject != null)
            {
                players[index] = playerObject.gameObject;
                playerObject.GetComponent<MultiPlayerMovement>().gameStart = true;
                playerObject.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[index].transform.localPosition);
            }
            index++;     
        }
        InitTurn();
    }


    public void InitTurn()
    {
        foreach (GameObject ele in players)
        {
            if(ele.GetComponent<MultiPlayerMovement>().myTurn)
            {
                currrentTurn = ele.GetComponent<MultiPlayerMovement>().playerId;
                break;
            }
        }
    }


}
