using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class MultiPlayManager : MonoBehaviour
{
    public GameObject UserInfoUI;
    public Text InputUserName;
    public NetworkRunner networkRunner;
    public GameObject ballPrefab;
    private NetworkObject ball;
    void Start()
    {
        // NetworkRunner ???? ?? ????
        // networkRunner = GetComponent<NetworkRunner>();

        // ?? ????
        //SpawnBall();
    }

    public void SpawnBall()
    {
        Vector3 ballPosition = new Vector3(0f, 1f, 0f);
        ball = networkRunner.Spawn(ballPrefab, ballPosition, Quaternion.identity, null);
    }
    public void InputData()
    {
        InputUserName.text = UserInfoUI.transform.GetChild(1).GetComponent<InputField>().text;
        UserInfoUI.SetActive(false);
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "MyRoom"
        });
    }
}
