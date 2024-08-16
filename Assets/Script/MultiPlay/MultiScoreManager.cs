using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;

    private MultiPlayManager multiPlayManager;

    public MultiUIManager multiUIManager;

    public GameObject[] setPosition;

    public GameObject[] players = new GameObject[2];
    public GameObject[] networkPlayers = new GameObject[2];

    private float waitTime = 1f;

    public string currrentTurn;

    public int user0Score { set; get; } = 0;
    public int user1Score { set; get; } = 0;

    public bool isGameSet { set; get; } = false;

    [Networked]
    public int OkCount { get; set; } = 0;
    public bool PushOkBtn = false;

    public override void Spawned()
    {
        base.Spawned();
        networkRunner = FindObjectOfType<NetworkRunner>();
        setPosition = GameObject.FindGameObjectsWithTag("PositionSetting");
        multiUIManager = GameObject.Find("MultiUIManager").GetComponent<MultiUIManager>();
        multiPlayManager = GameObject.FindObjectOfType<MultiPlayManager>();
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
            if(ele != null)
            {
                if (ele.GetComponent<MultiPlayerMovement>().myTurn)
                {
                    currrentTurn = ele.GetComponent<MultiPlayerMovement>().playerId;
                    ele.GetComponent<MultiPlayerMovement>().playerCode = 0;
                    ele.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[0].transform.localPosition);
                    ele.GetComponent<MultiPlayerMovement>().RpcMoveToRotation(new Vector3(0f, 180f, 0f));
                    // Trun Back Need
                    RPCUserIdUi(ele.GetComponent<MultiPlayerMovement>().playerId, 0);
                    networkPlayers[0] = ele;
                }
                else
                {
                    ele.GetComponent<MultiPlayerMovement>().playerCode = 1;
                    ele.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[1].transform.localPosition);
                    RPCUserIdUi(ele.GetComponent<MultiPlayerMovement>().playerId, 1);
                    networkPlayers[1] = ele;
                }
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_changeTurn()
    {
        foreach (GameObject ele in networkPlayers)
        {
            if(ele.GetComponent<MultiPlayerMovement>().playerId != currrentTurn)    
            {
                ele.GetComponent<MultiPlayerMovement>().ResetMyTurn();
                currrentTurn = ele.GetComponent<MultiPlayerMovement>().playerId;
                break;
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCScoreWinner(int winUserCode) { 
        if (!isGameSet)
        {
            if (winUserCode == 0) user0Score += 1;
            else if (winUserCode == 1) user1Score += 1;
            multiUIManager.UpdateScoreUI(user0Score, user1Score);
            multiUIManager.GameSet(true, winUserCode);
            isGameSet = true;
            StartCoroutine(WaitGameSetTime());
        }
        //Change User Turn
    }

    IEnumerator WaitGameSetTime()
    {
        yield return new WaitForSeconds(1f);
        multiUIManager.PrintTimeUI(5);
        yield return new WaitForSeconds(1f);
        multiUIManager.PrintTimeUI(4);
        yield return new WaitForSeconds(1f);
        multiUIManager.PrintTimeUI(3);
        yield return new WaitForSeconds(1f);
        multiUIManager.PrintTimeUI(2);
        yield return new WaitForSeconds(1f);
        multiUIManager.PrintTimeUI(1);
        yield return new WaitForSeconds(1f);
        RPCNextGame();
    }

    public void RPCNextGame()
    {
        isGameSet = false;
        OkCount = 0;
        PushOkBtn = false;
        multiUIManager.GameSetOff();
        if (multiPlayManager.ball != null)
        {
            RPC_changeTurn();
            networkRunner.Despawn(multiPlayManager.ball);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCUserIdUi(string UserId, int UserCode)
    {
        multiUIManager.UpdateUserId(UserId, UserCode);
    }
}