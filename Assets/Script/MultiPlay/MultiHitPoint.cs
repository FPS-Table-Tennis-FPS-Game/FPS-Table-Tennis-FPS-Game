using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHitPoint : MonoBehaviour
{

    [HideInInspector]
    public int swingType;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;

            float guagePower = 0.5f;
            //�÷��̾��� ���� ���� �� ������ : ī�޶� ���� �������� �� ������
            float force = guagePower * 1000;


            //���� ���� ������ ���� ���� ���� �� ����
            float upPower = 0;
            if (swingType == 0)
            {
                upPower = guagePower * 350;
            }
            else if (swingType == 1)
            {
                upPower = guagePower * 600;
            }


            //���� ������� ��� ���
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * upPower);

            //����Ȯ�� + ���� �̴�(ġ��) ��  
            other.GetComponent<Rigidbody>().AddForce(transform.parent.forward * force);

        }
    }
}
