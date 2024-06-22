using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovController : MonoBehaviour
{
    public float speed = 3.5f;
    public Vector3 movement;
    public Rigidbody PlayerRigidBody;
    public CameraController Camera;
    public Animator PlayerAnim;
    public BoxCollider StrokeRange;
    private UIcontroller uIcontroller;

    private BallController ballController;
    private bool isCharged = false;
    private bool isCharging = false;


    private Vector3 stopState = new Vector3(0f,0f,0f);
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerRigidBody.position = movement;
        GameObject canvas = GameObject.Find("Canvas");
        uIcontroller = canvas.GetComponent<UIcontroller>();

        ballController = GameObject.Find("Ball Director").GetComponent<BallController>();
    }

    void Update()
    {
        Run();
        //Mouse button press > charge > mouse button up > hit  > Gauge reset
        if (Input.GetMouseButtonDown(0))
        { 
            if (isCharged)
            {
                StrokeRange.enabled = true;
                PlayerAnim.SetTrigger("Swing");                          
                isCharged = false;  // Reset charge status after shooting
            }
            else
            {                
                if (!isCharging)
                {
                    StartCoroutine(WaitCharge());
                    isCharging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0) && isCharging)   
            uIcontroller.AddGauge(0.01f);

        else if (Input.GetMouseButtonUp(0))
        {
            StrokeRange.enabled = false;
          
            if (isCharging)
            {
                isCharged = true;
                isCharging = false;
            }

            if(!isCharged)
            uIcontroller.ResetGauge();
        }
    }

   
    IEnumerator WaitStroke()
    {
        yield return new WaitForSeconds(0.04f);
        StrokeRange.enabled = true;
        yield return new WaitForSeconds(0.02f);
        StrokeRange.enabled = false;
    }

    IEnumerator WaitCharge()
    {
        yield return new WaitForSeconds(2f);
    }

    void Run()
    {
        if(ballController.servState == false)
        {
            Turn();
            Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (inputMoveXZ != stopState)
            {
                PlayerAnim.SetBool("Running", true);
            }
            else
            {
                PlayerAnim.SetBool("Running", false);
            }

            float inputMoveXZMgnitude = inputMoveXZ.sqrMagnitude;

            inputMoveXZ = transform.TransformDirection(inputMoveXZ);

            if (inputMoveXZMgnitude <= 1)
                inputMoveXZ *= speed;
            else
                inputMoveXZ = inputMoveXZ.normalized * speed;

            movement = inputMoveXZ;
            movement = movement * Time.deltaTime;
            PlayerRigidBody.MovePosition(transform.position + movement);
        }
    }

    void Turn()
    {
        Quaternion cameraRotation = Camera.transform.rotation;

        cameraRotation.x = cameraRotation.z = 0;

        if (movement != Vector3.zero)
        {
            PlayerRigidBody.rotation = Quaternion.Slerp(PlayerRigidBody.rotation, cameraRotation, 10.0f * Time.deltaTime).normalized;
        }
    }

    public Quaternion PlayerRotation()
    {
        return Camera.transform.rotation;
    }
    void FixedUpdate()
    {
        Run();
    }

    }
