using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;
    public GameObject[] players;
    public GameObject[] setPosition;

    [Networked]
    public string user1 { get; set; }
    [Networked]
    public string user2 { get; set; }

    private float waitTime = 1f;

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
            NetworkObject playerObject = Runner.GetPlayerObject(player);
            if (playerObject != null)
            {
                if(index == 0)
                {
                    user1 = playerObject.GetComponent<MultiPlayerMovement>().playerId;
                } else
                {
                    user2 = playerObject.GetComponent<MultiPlayerMovement>().playerId;
                }
                playerObject.GetComponent<MultiPlayerMovement>().gameStart = true;
                playerObject.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[index].transform.localPosition);
            }
            index++;
        }
    }

    public void SetUsersPosition()
    {
        //StartCoroutine(SetPosition(waitTime));
    }

    IEnumerator SetPosition(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log(setPosition[i].transform.position);
            //
            players[i].GetComponent<MultiPlayerMovement>().gameStart = true;
        }
    }
}
