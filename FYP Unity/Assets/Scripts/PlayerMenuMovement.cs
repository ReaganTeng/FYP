using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform orientation;
    [SerializeField] float PlayerSpeed;
    [SerializeField] float DashCD;
    [SerializeField] float DashBy;
    bool DisableControls = false;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;


    private float dashcdtimer;
    private float Forwardrun;
    private float Rightrun;

    float delaytime;

    //
    bool isWalking;



    private void Start()
    {
        delaytime = 0.0f;
        isWalking = false;
        dashcdtimer = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (!DisableControls
            && GetComponentInChildren<Animator>().GetBool("click") == false
            )
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            playerRB.AddForce((orientation.right * horizontalInput) * PlayerSpeed);

            GetComponentInChildren<Animator>().SetBool("walking", isWalking);


            // If user is pressing any movement keys
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                //isWalking = true;
                //Debug.Log("MOVING");
                CheckDirection(ref Rightrun, horizontalInput);
            }


            if (Input.GetKey(KeyCode.A))
            {
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = true;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if (dashcdtimer > 0)
            {
                dashcdtimer -= Time.deltaTime;
            }
            else
            {
                // if shift is detected, sprint towards that direction
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    playerRB.AddForce((orientation.forward * Forwardrun + orientation.right * Rightrun) * PlayerSpeed * DashBy);
                    dashcdtimer = DashCD;

                }
            }
        }
    }




    public void DisablePlayerControls()
    {
        DisableControls = true;
    }

    public void EnablePlayerControls()
    {
        DisableControls = false;
    }

    void CheckDirection(ref float right, float horizontalInput)
    {
       
        //for player to move sideways
        if (horizontalInput > 0)
            right = 1;

        else if (horizontalInput < 0)
            right = -1;

        else
            right = 0;

    }

}