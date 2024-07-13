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

    [SerializeField]
    private PlayerManager playerManager;

    public void PlayerJoined(PlayerRef player)
    {
        NetworkObject spawnedUser = null;
        if (player == Runner.LocalPlayer)
        {
            PlayerRef playerRef = Runner.LocalPlayer;
            spawnedUser = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, playerRef);
        }

        if(Runner.ActivePlayers.Count() == 1)
        {
            gameObject.GetComponent<MultiPlayManager>().SpawnBall();
        }
    }

}
