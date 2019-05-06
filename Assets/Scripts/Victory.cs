using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //player has sucessfully collected the key
        if(other.tag == "Player" && player.GetComponent<PlayerControl>().key)
        {
            //TODO: Load Victory screen here
        }
    }
}
