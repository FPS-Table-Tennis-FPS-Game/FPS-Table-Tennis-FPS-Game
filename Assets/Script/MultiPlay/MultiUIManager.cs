using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class MultiUIManager : MonoBehaviour
{
    public Text UserScore0;
    public Text UserScore1;

    public Text UserId0;
    public Text UserId1;

    public void UpdateScoreUI(int userScore0, int userScore1)
    {
        UserScore0.text = userScore0.ToString();
        UserScore1.text = userScore1.ToString();
    }

    public void UpdateUserId(string userId, int userCode)
    {
        if (userCode == 0) UserId0.text = userId;
        else if (userCode == 1) UserId1.text = userId;
    }

}
