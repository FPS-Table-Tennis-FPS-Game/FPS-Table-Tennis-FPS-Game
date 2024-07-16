using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;
    public GameObject[] players;

    [Networked]
    public string user1 { get; set; }
    [Networked]
    public string user2 { get; set; }

    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        StartCoroutine(FindPlayers(1f));
    }

    IEnumerator FindPlayers(float time)
    {
        yield return new WaitForSeconds(time);

        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            if (i == 0) user1 = players[i].GetComponent<MultiPlayerMovement>().playerId;
            if (i == 1) user2 = players[i].GetComponent<MultiPlayerMovement>().playerId;
        }
    }
}
