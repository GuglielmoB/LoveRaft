using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControl : MonoBehaviour
{
    public float MoveSpeed;
    public float LookSpeed;

    public bool ForceEnableVerticalLook;
    public Camera PlayerCamera;

    void Update()
    {
        var direction = Quaternion.Euler(0, PlayerCamera.transform.rotation.eulerAngles.y, 0);
        transform.position += Input.GetAxis("Strafe") * (direction * Vector3.right) * MoveSpeed;
        transform.position += Input.GetAxis("Thrust") * (direction * Vector3.forward) * MoveSpeed;

        transform.Rotate(transform.up, Input.GetAxis("Camera Horizontal") * LookSpeed);
        // only rotate the camera itself up/down -- automatically doesn't work if VR is up
        PlayerCamera.transform.Rotate(-Input.GetAxis("Camera Vertical") * LookSpeed, 0, 0);
    }
}
