using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

//IplayerJoined : 플레이어가 세션에 참여할 때마다 호출되는 함수가 있음 (PlayerJoined)
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            PlayerRef playerRef = Runner.LocalPlayer;
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, playerRef);
        }

        if(Runner.ActivePlayers.Count() == 1)
        {
            gameObject.GetComponent<MultiPlayManager>().SpawnBall();
        }
    }

}
