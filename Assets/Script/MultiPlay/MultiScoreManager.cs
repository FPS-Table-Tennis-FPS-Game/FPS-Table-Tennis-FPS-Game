using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;

    public MultiUIManager multiUIManager;

    public GameObject[] setPosition;

    public GameObject[] players = new GameObject[2];

    private float waitTime = 1f;

    public string currrentTurn;

    [Networked] public int user0Score { set; get; } = 0;
    [Networked] public int user1Score { set; get; } = 0;

    public override void Spawned()
    {
        base.Spawned();
        networkRunner = FindObjectOfType<NetworkRunner>();
        setPosition = GameObject.FindGameObjectsWithTag("PositionSetting");
        multiUIManager = GameObject.Find("MultiUIManager").GetComponent<MultiUIManager>();
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
                ele.GetComponent<MultiPlayerMovement>().playerCode = 0;
                ele.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[0].transform.localPosition);
            } else
            {
                ele.GetComponent<MultiPlayerMovement>().playerCode = 1;
                ele.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[1].transform.localPosition);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCScoreWinner(int winUserCode)
    {
        if (winUserCode == 0) user0Score += 1;
        else if(winUserCode == 1) user1Score += 1;

        multiUIManager.UpdateScoreUI(user0Score, user1Score);

        //Change User Turn
    }
}
