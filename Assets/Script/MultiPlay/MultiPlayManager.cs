using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiPlayManager : MonoBehaviour
{
    public NetworkRunner networkRunner;
    public GameObject ballPrefab;
    private NetworkObject ball;
    void Start()
    {
        // NetworkRunner ???? ?? ????
        // networkRunner = GetComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "MyRoom"
        });

        // ?? ????
        //SpawnBall();
    }

    public void SpawnBall()
    {
        Vector3 ballPosition = new Vector3(0f, 1f, 0f);
        ball = networkRunner.Spawn(ballPrefab, ballPosition, Quaternion.identity, null);
    }
}
