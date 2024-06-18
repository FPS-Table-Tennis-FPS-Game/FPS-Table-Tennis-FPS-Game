using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointManager : MonoBehaviour
{
    private UIcontroller uIcontroller;
    private BallController ballController;
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        uIcontroller = canvas.GetComponent<UIcontroller>();
        GameObject ballDirector = GameObject.Find("Ball Director");
        ballController = ballDirector.GetComponent<BallController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            
            //플레이어의 방향 으로 공 보내기 : 카메라 각도 방향으로 공 보내기
            float force = uIcontroller.GetCurrentGauge() * 1000;
            other.GetComponent<Rigidbody>().AddForce(transform.parent.forward * force);
            Destroy(ballController.generatedFrame);

        }
    }
}
