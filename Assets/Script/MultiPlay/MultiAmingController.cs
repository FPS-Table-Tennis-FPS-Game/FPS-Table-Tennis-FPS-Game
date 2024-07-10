using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAmingController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    //private HitPointManager hitPManager; 

    private void Start()
    {
        //GameObject player = GameObject.Find("StrokeRange");
        //hitPManager= player.GetComponent<HitPointManager>();
    }


    public void Run(bool _flag) 
    {
        animator.SetBool("Run",_flag);
    }
    public void Idle(bool _flag)
    {
        animator.SetBool("Idle", _flag);
    }
    public void Hitball() 
    {
        if (animator.GetBool("Run"))
        {
            animator.SetTrigger("Run_Hit");
            
            //???? ???? ?? ???????? ???? 
           /* for (int i = 0; i < hitPManager.inaccuracy.Length; i++)
                hitPManager.inaccuracy[i] = UnityEngine.Random.Range(-10, 10) * 10; 
           */

        }
        else
        { 
            animator.SetTrigger("Idle_Hit");
            // ???????? ????
            //hitPManager.inaccuracy = new int[3] {0,0,0 };
        }
    }
}
