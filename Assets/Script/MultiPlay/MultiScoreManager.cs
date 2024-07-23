using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiScoreManager : NetworkBehaviour
{
    public NetworkRunner networkRunner;

    public GameObject[] setPosition;

    /*
    [Networked]
    public string user1 { get; set; }
    [Networked]
    public string user2 { get; set; }
    */

    public Dictionary<string, GameObject> playersDic = new Dictionary<string, GameObject>();

    private float waitTime = 1f;

    public override void Spawned()
    {
        base.Spawned();
       // networkRunner = FindObjectOfType<NetworkRunner>();
       // setPosition = GameObject.FindGameObjectsWithTag("PositionSetting");
        //StartCoroutine(FindPlayers(waitTime));
    }

    IEnumerator FindPlayers(float time)
    {
        yield return new WaitForSeconds(time);

        int index = 0;
        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            string playerKey = "player" + index;
            NetworkObject playerObject = Runner.GetPlayerObject(player);

            if (playerObject != null)
            {
                playersDic.Add(playerKey, playerObject.gameObject);
                playerObject.GetComponent<MultiPlayerMovement>().gameStart = true;
                playerObject.GetComponent<MultiPlayerMovement>().RpcMoveToPosition(setPosition[index].transform.localPosition);
                Debug.Log("player" + index + " " + playerObject.GetComponent<MultiPlayerMovement>().playerId);
                Debug.Log(setPosition[index].transform.localPosition);
            }
            index++;
        }
        //SetTurn();
    }

    public void SetTurn()
    {
        Debug.Log("player0: " + playersDic["player0"].GetComponent<MultiPlayerMovement>().playerId);
      //  player1.GetComponent<MultiPlayerMovement>().myTurn = true;
    }
}
