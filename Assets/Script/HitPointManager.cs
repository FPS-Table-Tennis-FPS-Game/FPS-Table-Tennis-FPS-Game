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
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        uIcontroller = canvas.GetComponent<Gaugecontroller>();
        aimingController = canvas.GetComponent<AimingController>();

        GameObject ballDirector = GameObject.Find("Ball Director");
        ballController = ballDirector.GetComponent<BallController>();

        // ���͸� ���� x,y,z ��
        inaccuracy = new int[3]{ 0,0,0};
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;

            //���� �Ʊ⿡ ���̹� ����Ʈ �����        
            aimingController.Hitball();

            //����Ȯ ����
            Vector3 distubrance = new Vector3((float)inaccuracy[0], (float)inaccuracy[1], (float)inaccuracy[2]);

            //�÷��̾��� ���� ���� �� ������ : ī�޶� ���� �������� �� ������
            float force = uIcontroller.GetCurrentGauge() * 1000;

            //���� ������� ��� ���
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);

            //����Ȯ�� + ���� �̴�(ġ��) ��  
            other.GetComponent<Rigidbody>().AddForce(distubrance + transform.parent.forward * force);
       

            //���� �̴� ���� ���ϸ� ���� ���� �����
            //���� �̴� ���� ���ϸ� ���� ���� �����
            //�������� �׽�Ʈ�� ���� �뷱�� ���� ����

            

            if (ballController.servState == true)
            {
                Destroy(ballController.generatedFrame);
                ballController.servState = false;
            }
        }
    }
}
