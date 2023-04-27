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
    bool DisableControls;

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
        DisableControls = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if hurt animation is playing 
        /*if(GetComponentInChildren<Animator>().GetBool("Hurt"))
        {
            delaytime += Time.deltaTime;
            if (delaytime >= 0.9f)
            {
                DisableControls = true;
            }
        }
        //
        else
        {
            DisableControls = false;   
        }*/

        if (!DisableControls
            && GetComponentInChildren<Animator>().GetBool("click") == false
            )
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);

            GetComponentInChildren<Animator>().SetBool("walking", isWalking);


            // If user is pressing any movement keys
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                //isWalking = true;
                //Debug.Log("MOVING");
                CheckDirection(ref Forwardrun, ref Rightrun, horizontalInput, verticalInput);
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

}