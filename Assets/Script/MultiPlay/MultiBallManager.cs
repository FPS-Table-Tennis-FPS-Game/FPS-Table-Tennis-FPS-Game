using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiBallManager : NetworkBehaviour
{
    public int attackerCode;
    public bool serveBall = true;

    [Networked]
    public MultiScoreManager multiScoreManager { get; set; }

    [Networked]
    public int dropTableCnt { get; set; } = 0;

    public override void Spawned()
    {
        multiScoreManager =  GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MultiScoreManager>();
        attackerCode = 99;
    }

    //When Serve ball
    public override void FixedUpdateNetwork()
    {
        if(serveBall)
        {

        }
    }

    public void CheckHit(GameObject user)
    {
        attackerCode = user.GetComponent<MultiPlayerMovement>().playerCode;
        serveBall = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(attackerCode != 99 && !multiScoreManager.isGameSet)
        {
            if (collision.gameObject.tag == "Table")
            {
                dropTableCnt += 1;
            }
            else if (collision.gameObject.tag == "Ground")
            {
                // Drop ball on the ground
                if (attackerCode == 1) multiScoreManager.RPCScoreWinner(0);
                else if (attackerCode == 0) multiScoreManager.RPCScoreWinner(1);
            }
        }
    }
}
