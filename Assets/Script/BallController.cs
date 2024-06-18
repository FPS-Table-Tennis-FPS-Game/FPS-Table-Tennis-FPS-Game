using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    GameObject player;
    GameObject ball;
    GameObject generatedBall;
    GameObject frame;
    public GameObject generatedFrame;
    Rigidbody ballRigdbody;

    bool sensorActivated = false; // ���� Ȱ��ȭ ���θ� ��Ÿ���� ����

    public float tossForce;
    float balloldPos = 0;
    float ballnewPos = 0;

    void Start()
    {
        ball = Resources.Load<GameObject>("Prefabs/Ball");
        player = GameObject.Find("Player");
        frame = Resources.Load<GameObject>("Prefabs/Frame");
        generatedBall = null;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            if (generatedBall == null)
            {
                FrameInstant(playerPos);
                Ballinstant(playerPos);              
                ballnewPos = generatedBall.transform.position.y;
                StartCoroutine(ActivateSensor());
            }
        }
        else
            ballnewPos = generatedBall.transform.position.y;

        if (generatedFrame != null)
        {
            if (balloldPos - ballnewPos == 0)
            {
                ballRigdbody.AddForce(Vector3.up * tossForce*Time.deltaTime, ForceMode.Impulse);
            }
            balloldPos = ballnewPos;
            if (sensorActivated)
            {
                BallspeedDown(playerPos);
            }
        }
       
    }
    
    void Ballinstant(Vector3 playerPos)
    {
        Vector3 ballPos = new Vector3(0, 0, 1f);
        generatedBall = Instantiate(ball, playerPos + ballPos, Quaternion.identity);
        ballRigdbody = generatedBall.GetComponent<Rigidbody>();
    }

    void FrameInstant(Vector3 playerPos)
    {
        Vector3 framePos = new Vector3(0, 0, 1f);
        generatedFrame = Instantiate(frame, playerPos + framePos, Quaternion.identity);
    }

    IEnumerator ActivateSensor()
    {
        yield return new WaitForSeconds(0.5f); // 0.5�� �Ŀ� ���� Ȱ��ȭ
        sensorActivated = true;
    }

    void BallspeedDown(Vector3 playerPos)
    {
        Vector3 frameHeight = playerPos + new Vector3(0, 0.25f, 0f);
        Vector3 frameBottom = playerPos + new Vector3(0, -0.25f, 0f);

        if (ballnewPos - frameHeight.y < 0.1f) // ������ Ȱ��ȭ�ǰ� ���� ������ �Ʒ��� ����
        {
            StartCoroutine(SpeedDown(frameBottom));
            sensorActivated = false;
        }
    }
    IEnumerator SpeedDown(Vector3 frameBottom)
    {
        ballRigdbody.isKinematic = true;

        while (generatedBall.transform.position.y - frameBottom.y >= 0.1f)
        {
            generatedBall.transform.Translate(0, -0.3f * Time.deltaTime, 0);
            yield return null;
        }

        // �ڷ�ƾ ���� �� �߰� ������ �ʿ��ϴٸ� ���⼭ ó��
        ballRigdbody.isKinematic = false;
    }
}