using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiBallManager : NetworkBehaviour
{
    public string attacker;

    [Networked]
    public MultiScoreManager multiScoreManager { get; set; }

    public override void Spawned()
    {
        multiScoreManager =  GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MultiScoreManager>();
    }

    public void CheckHit(GameObject user)
    {
        attacker = user.GetComponent<MultiPlayerMovement>().playerId.ToString();
        Debug.Log(attacker);
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
    }
}
