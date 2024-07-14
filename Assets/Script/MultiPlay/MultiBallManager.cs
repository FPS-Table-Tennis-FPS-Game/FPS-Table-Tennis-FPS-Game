using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiBallManager : NetworkBehaviour
{
    public string attacker;
    public void CheckHit(GameObject user)
    {
        attacker = user.GetComponent<MultiPlayerMovement>().playerId.ToString();
        Debug.Log(attacker);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}
