using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    public Gate gateTrigger;
    bool active = false;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gateTrigger != null)
        {
            if (!active && gateTrigger.justTeleported)
            {
                active = true;
                Vector3 temp = transform.position;
                temp.y += offset;
                transform.position = temp;

            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //not sure how we want to actually open doors, gonna delete it for now?
            Destroy(door);
            Destroy(this.gameObject);
        }
    }
}
