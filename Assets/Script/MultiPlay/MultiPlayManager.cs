using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class MultiPlayManager : MonoBehaviour
{
    public GameObject UserInfoUI;
    
    public GameObject UserAimUI;
    public GameObject UserPowerUI;
    public GameObject ScoreUI;

    public Text InputUserName;
    public NetworkRunner networkRunner;

    public GameObject ballPrefab;

    private NetworkObject ball;

    public GameObject scorePrefab;

    [SerializeField]
    private NetworkObject multiScoreManager;

    [SerializeField]
    private string inputId;


    public void SpawnBall(Vector3 ballPos)
    {
        Debug.Log(ballPos);
        Vector3 ballPosition = ballPos + new Vector3(0f, 2f, 0f);
        ball = networkRunner.Spawn(ballPrefab, ballPosition, Quaternion.identity, null);
    }

    public void SpawnScoreManager()
    {
        multiScoreManager = networkRunner.Spawn(scorePrefab, new Vector3(0f, 0f, 0f));
    }

    public void InputData()
    {
        InputUserName.text = UserInfoUI.transform.GetChild(1).GetComponent<InputField>().text;
        inputId = InputUserName.text;

        UserInfoUI.SetActive(false);

        ScoreUI.SetActive(true);
        UserAimUI.SetActive(true);
        UserPowerUI.SetActive(true);

        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "MyRoom"
        });
    }
}
