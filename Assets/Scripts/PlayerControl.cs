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

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var direction = Quaternion.Euler(0, PlayerCamera.transform.rotation.eulerAngles.y, 0);
        rb.AddForce(Input.GetAxis("Strafe") * (direction * Vector3.right) * MoveSpeed);
        rb.AddForce(Input.GetAxis("Thrust") * (direction * Vector3.forward) * MoveSpeed);

        rb.AddTorque(0, Input.GetAxis("Camera Horizontal") * LookSpeed, 0);
        // only rotate the camera itself up/down -- automatically doesn't work if VR is up
        PlayerCamera.transform.Rotate(-Input.GetAxis("Camera Vertical") * LookSpeed, 0, 0);
    }
}
