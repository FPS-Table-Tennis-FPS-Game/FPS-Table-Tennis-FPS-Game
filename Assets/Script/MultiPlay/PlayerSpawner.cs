using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        NetworkObject spawnedUser = null;

        if (player == Runner.LocalPlayer)
        {   
            PlayerRef playerRef = Runner.LocalPlayer;
            spawnedUser = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, playerRef);
            if (Runner.ActivePlayers.Count() == 2)
            {
                gameObject.GetComponent<MultiPlayManager>().SpawnScoreManager();
                gameObject.GetComponent<MultiPlayManager>().SpawnBall();
            }
        }
    }

}
