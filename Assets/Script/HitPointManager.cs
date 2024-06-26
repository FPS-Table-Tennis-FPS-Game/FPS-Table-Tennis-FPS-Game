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

            //공을 어느정도 띄울 경우
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);

            //공을 미는(치는) 힘
            other.GetComponent<Rigidbody>().AddForce(transform.parent.forward * force);

            //공을 미는 힘이 약하면 공이 많이 띄워짐
            //공을 미는 힘이 강하면 공이 적게 띄워짐
            //지속적인 테스트를 통해 밸런스 조정 예정

            if(ballController.servState == true)
            {
                Destroy(ballController.generatedFrame);
                ballController.servState = false;
            }
        }
    }
}
