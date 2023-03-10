using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform orientation;
    [SerializeField] float PlayerSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);
    }
}
