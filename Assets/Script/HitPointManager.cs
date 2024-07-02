using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointManager : MonoBehaviour
{
    private Gaugecontroller uIcontroller;
    private BallController ballController;
    private AimingController aimingController;
    public int[] inaccuracy;

    [HideInInspector]
    public int swingType;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        uIcontroller = canvas.GetComponent<Gaugecontroller>();
        aimingController = canvas.GetComponent<AimingController>();

        GameObject ballDirector = GameObject.Find("Ball Director");
        ballController = ballDirector.GetComponent<BallController>();

        // 벡터를 위한 x,y,z 값
        inaccuracy = new int[3] { 0, 0, 0 };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;

            float guagePower = uIcontroller.GetCurrentGauge();
            //플레이어의 방향 으로 공 보내기 : 카메라 각도 방향으로 공 보내기
            float force = guagePower * 1000;


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

            //공을 쳤기에 에이밍 포인트 사라짐        
            aimingController.Hitball();

            //부정확 방향
            Vector3 distubrance = new Vector3((float)inaccuracy[0], (float)inaccuracy[1], (float)inaccuracy[2]);

            //공을 어느정도 띄울 경우
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * upPower);

            //부정확성 + 공을 미는(치는) 힘  
            other.GetComponent<Rigidbody>().AddForce(distubrance + transform.parent.forward * force);


            //공을 미는 힘이 약하면 공이 많이 띄워짐
            //공을 미는 힘이 강하면 공이 적게 띄워짐
            //지속적인 테스트를 통해 밸런스 조정 예정



            if (ballController.servState == true)
            {
                Destroy(ballController.generatedFrame);
                ballController.servState = false;
            }
        }
    }
}