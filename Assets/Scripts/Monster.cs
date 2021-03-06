﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //empty game objects used for path positions
    public List<GameObject> path;
    public GameObject currentTarget;
    public GameObject currentPortal;
    public int currIndex;
    public GameObject player;

    //monster movement vals
    //CharacterController characterController;
    Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;
    float startRotation;
    public float mass = 1f;
    public float maxSpeed = 1f;
    public float maxTurn = .5f;
    public float radius = 10f;

    //attack collider
    public BoxCollider attackCollider;

    //bool to see if monster is aggroed
    bool aggro;

    // Start is called before the first frame update
    void Start()
    {
        //initalize movement vals
        //this.characterController = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        aggro = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        //attack player
        if(other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerControl>().health -= 1;
            if(player.GetComponent<PlayerControl>().health <= 0)
            {
                //TODO: load endgame scene with option to restart game or go to main menu
            }
            else
            {
                currIndex++;
                if (currIndex > path.Count - 1)
                {
                    currIndex = 0;
                }
                currentPortal = path[currIndex];
                transform.position = currentPortal.transform.position;
                currentTarget = currentPortal.GetComponent<enemyPortal>().end;
            }
        }

    }

    protected void CalcSteering()
    {
        if (aggro)
        {
            //chase the player instead
            Vector3 pathForce = Seek(player.transform.position);
            if (pathForce.sqrMagnitude > maxTurn * maxTurn)
            {
                pathForce = pathForce.normalized * maxTurn;
            }
            ApplyForce(pathForce);
        }
        else
        {
            //follow predefined path
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
        force.y = 0;
        acceleration += force / mass;
    }

    protected bool DetectPlayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, player.transform.position-transform.position, out hit) && hit.collider.gameObject.tag == "Player")
        {
            return true;
        }
        

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, currentTarget.transform.position) < .1f && !aggro)
        {
            currIndex++;
            if(currIndex > path.Count-1)
            {
                currIndex = 0;
            }
            currentPortal = path[currIndex];
            transform.position = currentPortal.transform.position;
            currentTarget = currentPortal.GetComponent<enemyPortal>().end;
        }
        aggro = DetectPlayer();
        
        CalcSteering();

        velocity += acceleration;
        velocity = VectorHelper.Clamp(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);

        acceleration = Vector3.zero;
    }
}
