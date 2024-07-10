using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerManager : NetworkBehaviour
{
    [Networked]
    public int playerId { get; set; } = 0;

    public void PlayerIdInput(GameObject player)
    {
        player.GetComponent<MultiPlayerMovement>().playerId = playerId;
        playerId++;
    }
}
