using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    [Networked]
    public string user1 { get; set; }
    [Networked]
    public string user2 { get; set; }

    public override void Spawned()
    {
        GameObject[] user = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < user.Length; i++)
        {
            Debug.Log(user[i].GetComponent<MultiPlayerMovement>().playerId);
        }
        //user1 = user[0].GetComponent<MultiPlayerMovement>().playerId;
        //user2 = user[1].GetComponent<MultiPlayerMovement>().playerId;

    }
}
