using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 400f;
    public float dist = 4f;

    public float xSpeed = 220.0f;
    public float ySpeed = 100.0f;

    public float x = 0.0f;
    public float y = 0.0f;

    public float yMinLimit = 0f;//20f;
    public float yMaxLimit = 90f;//80f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        MouseMove();
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private void MouseMove()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        x += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        transform.rotation = rotation;
    }
}
