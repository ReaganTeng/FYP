using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        else if (Input.GetKey(KeyCode.S))
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * speed);
        else if (Input.GetKey(KeyCode.A))
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.right * speed);
        else if (Input.GetKey(KeyCode.D))
            gameObject.GetComponent<Rigidbody>().AddForce(transform.right * speed);
    }
}
