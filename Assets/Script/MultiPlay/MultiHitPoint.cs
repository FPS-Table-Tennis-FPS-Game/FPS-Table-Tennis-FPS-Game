using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiHitPoint : NetworkBehaviour
{
    [HideInInspector]
    public int swingType;

    [Networked]
    public float guagePower { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            NetworkId ballId = other.gameObject.GetComponent<NetworkObject>().Id;
            RPC_AddForceToBall(ballId);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_AddForceToBall(NetworkId ballId)
    {
        GameObject target = Runner.FindObject(ballId).gameObject;

        if (swingType == 0 || swingType == 1)
        {
            target.GetComponent<MultiBallManager>().CheckHit(transform.root.gameObject);
        }

        target.GetComponent<Rigidbody>().isKinematic = false;

        float force2 = guagePower * 1000;

        float upPower = 0;
        if (swingType == 0)
        {
            upPower = guagePower * 350;
        }
        else if (swingType == 1)
        {
            upPower = guagePower * 600;
        }
        target.GetComponent<MultiBallManager>().ChangeAuthority(Object.InputAuthority);

        target.GetComponent<Rigidbody>().AddForce(Vector3.up * upPower);
        target.GetComponent<Rigidbody>().AddForce(transform.parent.forward * force2);
    }
}
