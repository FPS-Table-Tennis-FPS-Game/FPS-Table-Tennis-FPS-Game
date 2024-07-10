using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovController : MonoBehaviour
{
    public const float speed = 3.5f;
    public Vector3 movement;
    public Rigidbody PlayerRigidBody;
    public CameraController Camera;
    public Animator PlayerAnim;
    public BoxCollider StrokeRange;
    public TrailRenderer RacketEffect;
    
    private Gaugecontroller uIcontroller;
    private AimingController aimingController;

    private BallController ballController;
    public bool ishit = false;
    private bool isCharging = false;


    private Vector3 stopState = new Vector3(0f, 0f, 0f);
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerRigidBody.position = movement;
        GameObject canvas = GameObject.Find("Canvas");
        uIcontroller = canvas.GetComponent<Gaugecontroller>();
        aimingController = canvas.GetComponent<AimingController>();
        ballController = GameObject.Find("Ball Director").GetComponent<BallController>();
    }
    void FixedUpdate()
    {
        Run();
    }
    void Update()
    {
        //Mouse button press > charge > mouse button up > hit  > Gauge reset
        if (Input.GetMouseButton(0))
        {
            //When Mouse clicked turn on swing effect
            uIcontroller.AddGauge(0.01f);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(WaitStrokeEffect());
            StartCoroutine(WaitStroke());
            PlayerAnim.SetTrigger("Swing");
            StartCoroutine(WaitReset());
        }

        //Mouse button press > charge > mouse button up > hit  > Gauge reset
        if (Input.GetMouseButton(1))
        {
            //When Mouse clicked turn on swing effect
            uIcontroller.AddGauge(0.01f);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(WaitStrokeEffect());
            StartCoroutine(WaitStroke());
            PlayerAnim.SetTrigger("BackSwing");
            StartCoroutine(WaitReset());
        }

    }

    IEnumerator WaitStrokeEffect()
    {
        if (RacketEffect.enabled == true)
        {
            yield return new WaitForSeconds(0.2f);
            RacketEffect.enabled = false;
        }
    }
    IEnumerator WaitStroke()
    {
        yield return new WaitForSeconds(0.04f);
        StrokeRange.enabled = true;
        yield return new WaitForSeconds(0.02f);
        StrokeRange.enabled = false;
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.3f);
        uIcontroller.ResetGauge();
    }

    void Run()
    {
        if (ballController.servState == false)
        {
            Turn();
            Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (inputMoveXZ != stopState)
            {
                PlayerAnim.SetBool("Running", true);
                aimingController.Run(true);
            }
            else
            {
                PlayerAnim.SetBool("Running", false);
                aimingController.Run(false);
            }

            // Left shift => dash
            float tempSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                
                tempSpeed = speed*3f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                tempSpeed = speed;
            }


            float inputMoveXZMgnitude = inputMoveXZ.sqrMagnitude;
            inputMoveXZ = transform.TransformDirection(inputMoveXZ);

            if (inputMoveXZMgnitude <= 1)
                inputMoveXZ *= speed;
            else
                inputMoveXZ = inputMoveXZ.normalized * tempSpeed;

           

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


}
