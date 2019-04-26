using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement control")]
    public float MoveSpeed;
    public float SprintMultiplier;
    public float MaxSprint;
    public float SprintRegenSpeed;
    public float SprintRegenDelay;
    private float sprintDelayLeft;
    private float sprintLeft;

    [HideInInspector]
    public float Exhaustion;

    [Header("Camera control")]
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
        sprintLeft = MaxSprint;
    }

    void Update()
    {
        var direction = Quaternion.Euler(0, PlayerCamera.transform.rotation.eulerAngles.y, 0);
        bool sprintInput = Input.GetButton("Sprint");
        if (sprintInput)
        {
            sprintDelayLeft = SprintRegenDelay;
            sprintLeft -= Time.deltaTime;
            if (sprintLeft < 0) sprintLeft = 0;
        }
        else
        {
            if (sprintDelayLeft <= 0)
            {
                sprintLeft += SprintRegenSpeed * Time.deltaTime;
                if (sprintLeft > MaxSprint) sprintLeft = MaxSprint;
            }
            else
            {
                sprintDelayLeft -= Time.deltaTime;
            }
        }
        Exhaustion = (MaxSprint - sprintLeft) / MaxSprint;
        var sprintMult = (sprintInput && sprintLeft > 0) ? SprintMultiplier : 1;
        rb.AddForce(Input.GetAxis("Strafe") * (direction * Vector3.right) * MoveSpeed * sprintMult);
        rb.AddForce(Input.GetAxis("Thrust") * (direction * Vector3.forward) * MoveSpeed * sprintMult);
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
            transform.Translate(camPos.x, 0, camPos.z, Space.World);
            UnityWorkaround.transform.position = workaroundPos;
        }
    }
}
