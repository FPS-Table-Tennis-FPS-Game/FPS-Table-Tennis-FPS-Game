using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fusion;
using UnityEngine.UI;

public class MultiPlayerMovement : NetworkBehaviour
{
    [Networked]
    public bool gameStart { get; set; }

    [Networked]
    public string playerId { get; set; }

    public Camera sight;
    public GameObject userHead;

    public float speed;
    public Vector3 movement;
    public Rigidbody PlayerRigidBody;
    public Animator PlayerAnim;
    public BoxCollider StrokeRange;
    public TrailRenderer RacketEffect;

    public MultiHitPoint userHitPoint;
    private MultiGuageController guageController;
    private MultiAmingController aimingController;

    private MultiPlayManager multiPlayManager;

    public bool ishit = false;
    public bool isSwing = false;

    public bool isLCharge = false;
    public bool isRCharge = false;

    private Vector3 stopState = new Vector3(0f, 0f, 0f);

    [Networked] public bool EffectEnabled { get; set; }

    // Start is called before the first frame update
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            sight = Camera.main;
            sight.GetComponent<FirstPersonCamera>().Target = gameObject.transform.GetChild(1).transform;

            GameObject canvas = GameObject.Find("Canvas");
            guageController = canvas.GetComponent<MultiGuageController>();
            aimingController = canvas.GetComponent<MultiAmingController>();
            multiPlayManager = GameObject.FindObjectOfType<MultiPlayManager>();

            playerId = canvas.transform.GetChild(3).GetComponentInChildren<Text>().text;

            EffectEnabled = false;

        }
    }


    public override void FixedUpdateNetwork()
    {
        if(gameStart)
        {
            if (HasStateAuthority == false)
            {
                return;
            }

            if (!EffectEnabled)
            {
                RacketEffect.enabled = false;
            }
            else
            {
                RacketEffect.enabled = true;
            }

            if (Input.GetAxis("SpawnBall") == 1)
            {
                multiPlayManager.SpawnBall(transform.position);
            }

            //Mouse button press > charge > mouse button up > hit  > Gauge reset
            if (Input.GetAxis("Fire2") != 1)
            {
                if (Input.GetAxis("Fire1") == 1 && !isLCharge)
                {
                    isLCharge = true;
                }
                else if (Input.GetAxis("Fire1") == 1 && isLCharge)
                {
                    guageController.AddGauge(0.04f);
                    if (userHitPoint.guagePower < 1f)
                    {
                        userHitPoint.guagePower += 0.04f;
                    }
                }
                else if (Input.GetAxis("Fire1") == 0 && isLCharge && !isSwing)
                {
                    isSwing = true;
                    userHitPoint.swingType = 0;
                    EffectEnabled = true;
                    StartCoroutine(WaitStrokeEffect());
                    StartCoroutine(WaitStroke());
                    PlayerAnim.SetTrigger("Swing");
                    StartCoroutine(WaitReset());
                }
            }

            if (Input.GetAxis("Fire1") != 1)
            {
                //Mouse button press > charge > mouse button up > hit  > Gauge reset
                if (Input.GetAxis("Fire2") == 1 && !isRCharge)
                {
                    isRCharge = true;
                }
                else if (Input.GetAxis("Fire2") == 1 && isRCharge)
                {
                    guageController.AddGauge(0.04f);
                    if (userHitPoint.guagePower < 1f)
                    {
                        userHitPoint.guagePower += 0.04f;
                    }
                }
                else if (Input.GetAxis("Fire2") == 0 && isRCharge && !isSwing)
                {
                    isSwing = true;
                    userHitPoint.swingType = 1;
                    EffectEnabled = true;
                    StartCoroutine(WaitStrokeEffect());
                    StartCoroutine(WaitStroke());
                    PlayerAnim.SetTrigger("BackSwing");
                    StartCoroutine(WaitReset());
                }
            }
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
    }

    IEnumerator WaitStrokeEffect()
    {
        if (EffectEnabled == true)
        {
            yield return new WaitForSeconds(0.2f);
            // RacketEffect.enabled = false;
            EffectEnabled = false;
        }
    }
    IEnumerator WaitStroke()
    {
        yield return new WaitForSeconds(0.02f);
        StrokeRange.enabled = true;
        yield return new WaitForSeconds(0.16f);
        StrokeRange.enabled = false;
        yield return new WaitForSeconds(0.02f);
        isSwing = false;
        isLCharge = false;
        isRCharge = false;
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.3f);
        guageController.ResetGauge();
        userHitPoint.guagePower = 0f;
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

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcMoveToPosition(Vector3 position)
    {
        transform.position = position;
    }
}
