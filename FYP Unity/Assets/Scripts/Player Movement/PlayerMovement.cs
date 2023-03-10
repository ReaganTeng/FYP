using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform orientation;
    [SerializeField] float PlayerSpeed;
    [SerializeField] float DashCD;
    [SerializeField] float DashBy;
    [SerializeField] float IFrame;
    private float dashcdtimer;
    private float Forwardrun;
    private float Rightrun;
    private float iframetimer;
    private bool IFrameStart;

    private void Start()
    {
        dashcdtimer = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);

        // If user is pressing any movement keys
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            CheckDirection(ref Forwardrun, ref Rightrun, horizontalInput, verticalInput);
        }

        if (dashcdtimer > 0)
            dashcdtimer -= Time.deltaTime;
        
        else
        {
            // if shift is detected, sprint towards that direction
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerRB.AddForce((orientation.forward * Forwardrun + orientation.right * Rightrun) * PlayerSpeed * DashBy);
                dashcdtimer = DashCD;
                iframetimer = IFrame;
                IFrameStart = true;
                Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
            }
        }
    }

    void CheckDirection(ref float foward, ref float right, float horizontalInput, float verticalInput)
    {
        // If player is moving foward
        if (verticalInput > 0)
            foward = 1;

        else if (verticalInput < 0)
            foward = -1;

        else
            foward = 0;

        if (horizontalInput > 0)
            right = 1;

        else if (horizontalInput < 0)
            right = -1;

        else
            right = 0;

    }

    private void Update()
    {
        if (IFrameStart)
        {
            if (iframetimer > 0)
                iframetimer -= Time.deltaTime;
            if (iframetimer <= 0)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
                IFrameStart = false;
            }
        }
    }
}
