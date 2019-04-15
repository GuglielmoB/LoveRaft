using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //empty game objects used for path positions
    public List<GameObject> path;
    public GameObject currentTarget;
    public GameObject lastTarget;
    public GameObject player;

    //monster movement vals
    CharacterController characterController;
    Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;
    float startRotation;
    public float mass = 1f;
    public float maxSpeed = 0.5f;
    public float maxTurn = 0.25f;
    public float radius = 10f;

    //bool to see if monster is aggroed
    bool aggro;

    // Start is called before the first frame update
    void Start()
    {
        //initalize movement vals
        this.characterController = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        aggro = false;

    }

    protected void CalcSteering()
    {
        if (aggro)
        {
            //chase the player instead
        }
        else
        {
            Vector3 pathForce = Seek(currentTarget.transform.position);
            if (pathForce.sqrMagnitude > maxTurn * maxTurn)
            {
                pathForce = pathForce.normalized * maxTurn;
            }
            ApplyForce(pathForce);
        }
        
    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - this.transform.position;
        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;

        return VectorHelper.Clamp(steeringForce, maxTurn);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    protected bool DetectPlayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, player.transform.position-transform.position, out hit) && hit.collider.gameObject == player)
        {
            return true;
        }
        

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        aggro = DetectPlayer();
        position = transform.position;
        CalcSteering();

        velocity += acceleration;
        velocity = VectorHelper.Clamp(velocity, maxSpeed);
        this.characterController.Move(velocity * Time.deltaTime);

        acceleration = Vector3.zero;
    }
}
