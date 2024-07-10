using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserManager : MonoBehaviour
{
    public GameObject space;
    public Transform shootPoint;
    public GameObject shootBall;
    public int bottomPower; // ���� ���� ����
    public int pushPower; // ���� �̴� ����
    private int ballCount = 0;

    void Start()
    {
        //�� �߻� ���� �κ�
        StartCoroutine(BallShoot());
    }
    private void OnTriggerEnter(Collider other)
    {
        // ���漭�� ���� Ÿ���Ͽ� ���� ������ ������ �����ϴ� ��� �߰� ���� 
        if (other.tag.Equals("Racket"))
        {
            Debug.Log("Dispenser Hit");
        }
    }

    IEnumerator BallShoot()
    {
        yield return new WaitForSeconds(4);
        shootBall.transform.position = shootPoint.position;
        GameObject instantBall = Instantiate(shootBall, space.transform);

        instantBall.GetComponent<Rigidbody>().AddForce(Vector3.up * bottomPower);
        instantBall.GetComponent<Rigidbody>().AddForce(transform.forward * pushPower);

        //�����ִ� ���� 10�� �̻��� ��� �����ִ� ���� ��� ������ ���� �ٽ� ����
        if (ballCount < 10)
        {
            StartCoroutine(BallShoot());
        } else
        {
            foreach (Transform addedBall in space.transform)
            {
                Destroy(addedBall.gameObject);
            }
            ballCount = 0;
            StartCoroutine(BallShoot());
        }
        ballCount += 1;
    }

}
