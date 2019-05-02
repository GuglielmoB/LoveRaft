using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    float xOffset = .1f;
    Vector3 tempPos;
    Vector3 endPos;
    bool active = false;
    public float speed = 1;
    public BoxCollider trigger;

    // Use this for initialization
    void Start () {

        tempPos = this.transform.position;
        endPos = tempPos;
        endPos.y += 3;
    }
	
	// Update is called once per frame
	void Update () {
		if(active && transform.position.y <= endPos.y)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;
            transform.position = temp;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        active = true;

    }
}
