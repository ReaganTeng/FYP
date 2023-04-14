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
    [SerializeField] PlayerPickup playerPickUp;
    bool DisableControls;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;


    private float dashcdtimer;
    private float Forwardrun;
    private float Rightrun;
    private float iframetimer;
    private bool IFrameStart;

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

        if (!DisableControls)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);

            GetComponentInChildren<Animator>().SetBool("walking", isWalking);

            // If user is pressing any movement keys
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                //Debug.Log("MOVING");
                CheckDirection(ref Forwardrun, ref Rightrun, horizontalInput, verticalInput);
            }
            else
            {
                isWalking = false;
            }

            if (Input.GetKey(KeyCode.A))
            { 
                isWalking = true;
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                spriteRenderer.flipX = true;
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
                    iframetimer = IFrame;
                    IFrameStart = true;
                    Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

                }
            }
        }
    }




    public void setAnimator(bool hurt)
    {
       GetComponentInChildren<Animator>().SetBool("Hurt", hurt);
    }

    public void DisablePlayerControls()
    {
        DisableControls = true;
        playerPickUp.DisableControls = true;
    }

    public void EnablePlayerControls()
    {
        DisableControls = false;
        playerPickUp.DisableControls = false;
    }

    public void SetSRColor(Color c)
    {
        spriteRenderer.color = c;
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

    public int GetDirection(float horizontalInput, float verticalInput)
    {
        // 1 suggest back, 2 suggest right, 3 suggest front, 4 suggest left
        // if moving towards the right
        if (horizontalInput > 0)
        {
            return 2;
        }

        else if (horizontalInput < 0)
        {
            return 4;
        }

        else if (verticalInput > 0)
        {
            return 1;
        }

        else if (verticalInput < 0)
        {
            return 3;
        }

        return 0;
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
