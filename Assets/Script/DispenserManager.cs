using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserManager : MonoBehaviour
{
    public GameObject space;
    public Transform shootPoint;
    public GameObject shootBall;
    public int bottomPower; // 공을 띄우는 정도
    public int pushPower; // 공을 미는 정도
    private int ballCount = 0;

    void Start()
    {
        //공 발사 시작 부분
        StartCoroutine(BallShoot());
    }
    private void OnTriggerEnter(Collider other)
    {
        // 디스펜서를 직접 타격하여 공이 나오는 방향을 조정하는 기능 추가 예정 
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

        //나와있는 공이 10개 이상일 경우 나와있는 공은 모두 제거한 다음 다시 생성
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
