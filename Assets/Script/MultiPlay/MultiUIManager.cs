using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class MultiUIManager : NetworkBehaviour
{
    public Text UserScore0;
    public Text UserScore1;


    public void UpdateScoreUI(int userScore0, int userScore1)
    {
        UserScore0.text = userScore0.ToString();
        UserScore1.text = userScore1.ToString();
    }

}
