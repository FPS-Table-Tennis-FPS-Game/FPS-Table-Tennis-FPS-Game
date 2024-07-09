using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiHitPoint : NetworkBehaviour
{
    [HideInInspector]
    public int swingType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            NetworkId ballId = other.gameObject.GetComponent<NetworkObject>().Id;
            RPC_AddForceToBall(ballId,new Vector3(0,0,0));
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_AddForceToBall(NetworkId ballId, Vector3 force)
    {
        GameObject target = Runner.FindObject(ballId).gameObject;

        target.GetComponent<Rigidbody>().isKinematic = false;

        float guagePower = 0.5f;
        //플레이어의 방향 으로 공 보내기 : 카메라 각도 방향으로 공 보내기
        float force2 = guagePower * 1000;


        //라켓 스윙 유형에 따라 공을 띄우는 힘 결정
        float upPower = 0;
        if (swingType == 0)
        {
            upPower = guagePower * 350;
        }
        else if (swingType == 1)
        {
            upPower = guagePower * 600;
        }


        //공을 어느정도 띄울 경우
        target.GetComponent<Rigidbody>().AddForce(Vector3.up * upPower);

        //부정확성 + 공을 미는(치는) 힘  
        target.GetComponent<Rigidbody>().AddForce(transform.parent.forward * force2);
    }
}
