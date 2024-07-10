using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBallManager : MonoBehaviour
{
    public string attacker;

    public void CheckHit(GameObject user)
    {
        attacker = user.GetComponent<MultiPlayerMovement>().playerId.ToString();
        Debug.Log(attacker);
    }
}
