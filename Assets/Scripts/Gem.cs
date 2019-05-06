using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GameObject gate;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //destroy key
            Destroy(gameObject);

            //set player key to true
            player.GetComponent<PlayerControl>().key = true;

            //move gate
            Vector3 temp = gate.transform.position;
            temp.y -= 4.3f;
            gate.transform.position = temp;
        }
    }
}
