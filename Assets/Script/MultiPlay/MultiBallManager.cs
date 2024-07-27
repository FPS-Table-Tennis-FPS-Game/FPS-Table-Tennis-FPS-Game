using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiBallManager : NetworkBehaviour
{
    public int attackerCode;

    [Networked]
    public MultiScoreManager multiScoreManager { get; set; }

    [Networked]
    public int dropTableCnt { get; set; } = 0;

    public override void Spawned()
    {
        multiScoreManager =  GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MultiScoreManager>();
        attackerCode = 99;
    }

    public void CheckHit(GameObject user)
    {
        attackerCode = user.GetComponent<MultiPlayerMovement>().playerCode;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(attackerCode != 99)
        {
            if (collision.gameObject.tag == "Table")
            {
                dropTableCnt += 1;
            }
            else if (collision.gameObject.tag == "Ground")
            {
                if (attackerCode == 1) multiScoreManager.RPCScoreWinner(0);
                else if (attackerCode == 0) multiScoreManager.RPCScoreWinner(1);
            }
        }
    }
}
