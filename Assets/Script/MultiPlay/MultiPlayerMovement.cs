using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MultiPlayerMovement : NetworkBehaviour
{

    public Camera sight;
    public GameObject userHead;

    public float speed;
    public Vector3 movement;
    public Rigidbody PlayerRigidBody;
   // public CameraController cameraController;
    public Animator PlayerAnim;
    public BoxCollider StrokeRange;
    public TrailRenderer RacketEffect;

    //private HitPointManager userHitPoint;
    private Gaugecontroller uIcontroller;
    //private AimingController aimingController;

    //private BallController ballController;
    public bool ishit = false;

    private Vector3 stopState = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            sight = Camera.main;
            sight.GetComponent<FirstPersonCamera>().Target = gameObject.transform.GetChild(1).transform;
        }
    }
    private void Update()
    {
        //Mouse button press > charge > mouse button up > hit  > Gauge reset
        if (Input.GetMouseButton(0))
        {
            //When Mouse clicked turn on swing effect
            // uIcontroller.AddGauge(0.01f);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //userHitPoint.swingType = 0;
            RacketEffect.enabled = true;
            StartCoroutine(WaitStrokeEffect());
            StartCoroutine(WaitStroke());
            PlayerAnim.SetTrigger("Swing");
            StartCoroutine(WaitReset());
        }

        //Mouse button press > charge > mouse button up > hit  > Gauge reset
        if (Input.GetMouseButton(1))
        {
            //When Mouse clicked turn on swing effect
            // uIcontroller.AddGauge(0.01f);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            // userHitPoint.swingType = 1;
            RacketEffect.enabled = true;
            StartCoroutine(WaitStrokeEffect());
            StartCoroutine(WaitStroke());
            PlayerAnim.SetTrigger("BackSwing");
            StartCoroutine(WaitReset());
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        Turn();
        Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputMoveXZ != stopState)
        {
            PlayerAnim.SetBool("Running", true);
            //aimingController.Run(true);
        }
        else
        {
            PlayerAnim.SetBool("Running", false);
            // aimingController.Run(false);
        }

       
        float inputMoveXZMgnitude = inputMoveXZ.sqrMagnitude;

        inputMoveXZ = transform.TransformDirection(inputMoveXZ);

        
        /*
        if (inputMoveXZMgnitude <= 1)
            inputMoveXZ *= speed;
        else
            inputMoveXZ = inputMoveXZ.normalized;
*/
    
        movement = inputMoveXZ * Runner.DeltaTime * speed;
        //movement = movement * Runner.DeltaTime;//Time.deltaTime;
        PlayerRigidBody.MovePosition(transform.position + movement);
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
        yield return new WaitForSeconds(0.02f);
        StrokeRange.enabled = true;
        yield return new WaitForSeconds(0.08f);
        StrokeRange.enabled = false;
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.3f);
       // uIcontroller.ResetGauge();
    }

    void Turn()
    {
        Quaternion cameraRotation = sight.transform.rotation;

        cameraRotation.x = cameraRotation.z = 0;

        if (movement != Vector3.zero)
        {
            PlayerRigidBody.rotation = Quaternion.Slerp(PlayerRigidBody.rotation, cameraRotation, 10.0f * Runner.DeltaTime).normalized;
        }
    }

    public Quaternion PlayerRotation()
    {
        return sight.transform.rotation;
    }

}
