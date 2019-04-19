using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControl : MonoBehaviour
{
    public float MoveSpeed;
    [Header("Camera control options")]
    public float LookSpeed;
    [Range(0, 90)]
    public float VerticalLookLimit;
    private float negLookLimit;

    public bool ForceEnableVerticalLook;
    // Because Unity is awful, we need to have the Player object, then an empty, _then_ the Camera.
    // UnityWorkaround, named such because it's a workaround for Unity's inherent failures, is that middle parent.
    public GameObject UnityWorkaround;
    public Camera PlayerCamera;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        negLookLimit = 360 - VerticalLookLimit;
    }

    void Update()
    {
        var direction = Quaternion.Euler(0, PlayerCamera.transform.rotation.eulerAngles.y, 0);
        rb.AddForce(Input.GetAxis("Strafe") * (direction * Vector3.right) * MoveSpeed);
        rb.AddForce(Input.GetAxis("Thrust") * (direction * Vector3.forward) * MoveSpeed);
        transform.Rotate(0, Input.GetAxis("Camera Horizontal") * LookSpeed, 0);

        if (!XRSettings.enabled)
        { // non-VR controls (look up/down)
            // only rotate the camera itself up/down -- automatically doesn't work if VR is attached
            var camRot = PlayerCamera.transform.rotation.eulerAngles;
            var newAngle = camRot.x - Input.GetAxis("Camera Vertical") * LookSpeed;
            float clampedNewAngle;
            if (newAngle < VerticalLookLimit || newAngle > negLookLimit) clampedNewAngle = newAngle;
            else if (newAngle < 180) clampedNewAngle = VerticalLookLimit;
            else clampedNewAngle = negLookLimit;
            var diff = clampedNewAngle - camRot.x;
            PlayerCamera.transform.Rotate(diff, 0, 0);
        }
    }

    private void LateUpdate()
    {
        if (XRSettings.enabled)
        { // move the collider with the camera
            var camPos = PlayerCamera.transform.position - transform.position;
            var workaroundPos = UnityWorkaround.transform.position;
            Debug.Log($"camPos {camPos}");
            transform.Translate(camPos.x, 0, camPos.z, Space.World);
            UnityWorkaround.transform.position = workaroundPos;
        }
    }
}
