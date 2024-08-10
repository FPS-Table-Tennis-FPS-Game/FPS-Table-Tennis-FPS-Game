using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiBallManager : NetworkBehaviour
{
    public int attackerCode;
    public int beforeAttacker;
    public bool serveBall = true;

    [Networked]
    public MultiScoreManager multiScoreManager { get; set; }

    [Networked]
    public int dropTableCnt { get; set; } = 0;

    [Networked]
    public int dropTable0Cnt { get; set; } = 0;
    [Networked]
    public int dropTable1Cnt { get; set; } = 0;

    public override void Spawned()
    {
        multiScoreManager =  GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MultiScoreManager>();
        attackerCode = 99;
        beforeAttacker = 99;
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
        // if player hit balls
        attackerCode = user.GetComponent<MultiPlayerMovement>().playerCode;
        serveBall = false;
        RPC_ResetDropCnt();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ResetDropCnt()
    {
        dropTableCnt = 0;
        dropTable0Cnt = 0;
        dropTable1Cnt = 0;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_CheckDropTableCnt(int tableCode)
    {
        if (tableCode == 0) dropTable0Cnt += 1;
        else if (tableCode == 1) dropTable1Cnt += 1;

        dropTableCnt += 1;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(attackerCode != 99 && !multiScoreManager.isGameSet)
        {
            if (collision.gameObject.tag == "Table")
            {
                if (collision.gameObject.name.ToString().Contains("0")) RPC_CheckDropTableCnt(0);
                else if (collision.gameObject.name.ToString().Contains("1")) RPC_CheckDropTableCnt(1);

                // One hit the ball and drop table over 1
                if (dropTableCnt > 1)
                {
                    if (attackerCode == 1)
                    {
                        if (dropTable0Cnt > 1)
                        {
                            multiScoreManager.RPCScoreWinner(1);
                            RPC_ResetDropCnt();
                        }
                    }
                    else if (attackerCode == 0)
                    {
                        if (dropTable1Cnt > 1)
                        {
                            multiScoreManager.RPCScoreWinner(0);
                            RPC_ResetDropCnt();
                        }
                    }
                } 
                
                if(dropTableCnt == 1)
                {
                    if (attackerCode == 1)
                    {
                        if (dropTable1Cnt == 1)
                        {
                            multiScoreManager.RPCScoreWinner(0);
                            RPC_ResetDropCnt();
                        }
                    }
                    else if (attackerCode == 0)
                    {
                        if (dropTable0Cnt == 1)
                        {
                            multiScoreManager.RPCScoreWinner(1);
                            RPC_ResetDropCnt();
                        }
                    }
                }
            }
            else if (collision.gameObject.tag == "Ground")
            {
                if(dropTableCnt == 1)
                {
                    // Attack complete
                    if (attackerCode == 1)
                    {
                        if (dropTable0Cnt == 1) multiScoreManager.RPCScoreWinner(1);
                        else if (dropTable1Cnt == 1) multiScoreManager.RPCScoreWinner(0);
                    }
                    else if (attackerCode == 0)
                    {
                        if(dropTable1Cnt == 1) multiScoreManager.RPCScoreWinner(0);
                        else if (dropTable0Cnt == 1) multiScoreManager.RPCScoreWinner(1);
                    }
                } else
                {
                    // Drop ball on the ground
                    if (attackerCode == 1) multiScoreManager.RPCScoreWinner(0);
                    else if (attackerCode == 0) multiScoreManager.RPCScoreWinner(1);
                }
                RPC_ResetDropCnt();
            }
        }
    }
}
