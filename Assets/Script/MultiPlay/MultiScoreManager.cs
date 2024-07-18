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
        networkRunner = FindObjectOfType<NetworkRunner>();
        setPosition = GameObject.FindGameObjectsWithTag("PositionSetting");
        StartCoroutine(FindPlayers(waitTime));
    }

    IEnumerator FindPlayers(float time)
    {
        yield return new WaitForSeconds(time);
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            if (i == 0)
            {
                user1 = players[i].GetComponent<MultiPlayerMovement>().playerId;
            }

            if (i == 1)
            {
                user2 = players[i].GetComponent<MultiPlayerMovement>().playerId;
            }
        }
    }

    public void SetUsersPosition()
    {
        StartCoroutine(SetPosition(waitTime));
    }

    IEnumerator SetPosition(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log(setPosition[i].transform.position);
            players[i].transform.position = setPosition[i].transform.position;
        }
    }
}
