using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Front, Back,
    Left, Right,
    Top, Bottom,
}

public class Gate : MonoBehaviour
{
    public BoxCollider Entrance;
    public Direction EnterDirection;
    public BoxCollider Exit;
    public Direction ExitDirection;
    public Vector3 Rotation;
    public bool Bidirectional;

    bool justTeleported;
    Quaternion qRotation;
    Quaternion qiRotation;

    void Start()
    {
        Entrance.isTrigger = Exit.isTrigger = true;
        justTeleported = false;
        qRotation = Quaternion.Euler(Rotation);
        qiRotation = Quaternion.Euler(-Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (justTeleported)
        {
            justTeleported = false;
            return;
        }
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if (other.bounds.Intersects(Entrance.bounds))
        {
            other.gameObject.transform.position = Exit.bounds.center;
            other.gameObject.transform.Rotate(Rotation);
            if (rb != null)
            {
                rb.velocity = qRotation * rb.velocity;
            }
            justTeleported = true;
        }
        if (Bidirectional && other.bounds.Intersects(Exit.bounds))
        {
            other.gameObject.transform.position = Entrance.bounds.center;
            other.gameObject.transform.Rotate(-Rotation);
            if (rb != null)
            {
                rb.velocity = qiRotation * rb.velocity;
            }
            justTeleported = true;
        }
    }
}
