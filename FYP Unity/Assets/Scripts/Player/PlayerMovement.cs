using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform orientation;
    [SerializeField] float PlayerSpeed;
    [SerializeField] float DashBy;
    [SerializeField] float IFrame;
    [SerializeField] PlayerPickup playerPickUp;
    [SerializeField] PlayerAttack playerAttack;
    bool DisableControls;


    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField] float DashCD;
    float dashcdtimer;

    private float Forwardrun;
    private float Rightrun;
    private float iframetimer;
    private bool IFrameStart;

    float delaytime;

    //
    bool isWalking;
    bool walkFront;
    bool walkBack;

    // Minimap
    GameObject minimapCanvas;
   

    private void Start()
    {
        dashcdtimer = 0.0f;
        delaytime = 0.0f;
        isWalking = false;
        DisableControls = false;
        minimapCanvas = GameObject.FindGameObjectWithTag("MinimapCanvas");
        minimapCanvas.SetActive(false);
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

        notheavyattackingordashing();

        if (dashcdtimer > 0)
        {
            dashcdtimer -= Time.deltaTime;
        }

        if (!GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_knife")
            && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_pin")
            && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_spatula")
             //&& notheavyattackingordashing()
             && !playerAttack.attacking_or_not()
             )
        {
            GetComponentInChildren<Animator>().SetBool("walking", isWalking);
            GetComponentInChildren<Animator>().SetBool("WalkFront", walkFront);
            GetComponentInChildren<Animator>().SetBool("WalkBack", walkBack);
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("walking", false);
            GetComponentInChildren<Animator>().SetBool("WalkFront", false);
            GetComponentInChildren<Animator>().SetBool("WalkBack", false);
        }


        //BACKWALK
        if (Input.GetKey(KeyCode.W)
                && !Input.GetKey(KeyCode.S)
                && !Input.GetKey(KeyCode.A)
                && !Input.GetKey(KeyCode.D)
                && !Input.GetKeyDown(KeyCode.LeftShift)
               //&& notheavyattackingordashing()
               && !playerAttack.attacking_or_not()
               )
        {
            walkBack = true;
        }
        else
        {
            walkBack = false;
        }
        //
        //FRONT WALK
        if (!Input.GetKey(KeyCode.W)
                && Input.GetKey(KeyCode.S)
                && !Input.GetKey(KeyCode.A)
                && !Input.GetKey(KeyCode.D)
                && !Input.GetKeyDown(KeyCode.LeftShift)
               //&& notheavyattackingordashing()
                && !playerAttack.attacking_or_not()
               )
        {
            walkFront = true;
        }
        else
        {
            walkFront = false;
        }
        //

        if (Input.GetKey(KeyCode.A)
             && notheavyattackingordashing()
            )
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.D)
             //&& notheavyattackingordashing()
             && !playerAttack.attacking_or_not()
             )
        {
            spriteRenderer.flipX = true;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            && !Input.GetKeyDown(KeyCode.LeftShift)
           //&& notheavyattackingordashing()
           && !playerAttack.attacking_or_not()

            )
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash")
            && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            GetComponentInChildren<Animator>().SetBool("Dash", false);
        }

        if (Input.GetKeyDown(KeyCode.M) && !DisableControls)
        {
            if (minimapCanvas.activeSelf)
            {
                minimapCanvas.SetActive(false);
            }
            else
            {
                minimapCanvas.SetActive(true);
            }
        }
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

        //notattackingordashing();

        if (!DisableControls
            && GetComponentInChildren<Animator>().GetBool("click") == false
            )
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // If user is pressing any movement keys
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            )
            {
                if (!playerAttack.attacking_or_not()
                    /*notheavyattackingordashing()*/)
                {
                    playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);
                    CheckDirection(ref Forwardrun, ref Rightrun, horizontalInput, verticalInput);
                    playerAttack.resetattacking();
                }
            }

            // if shift is detected, sprint towards that direction
            if (Input.GetKeyDown(KeyCode.LeftShift) &&
                dashcdtimer <= 0)
            {
                playerRB.AddForce((orientation.forward * Forwardrun + orientation.right * Rightrun) * PlayerSpeed * DashBy);
                iframetimer = IFrame;
                IFrameStart = true;
                GetComponentInChildren<Animator>().SetBool("Dash", true);
                //GetComponent<BoxCollider>().enabled = false;
                Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
                dashcdtimer = DashCD;
                GameSoundManager.PlaySound("Dash");
            }
            
        }
    }


    
    public bool notheavyattackingordashing()
    {
        if(!GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash")
        /*&& !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_knife")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_pin")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_spatula")*/
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_knife")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin"))
        {
            return true;
        }
        else
        {
            return false;
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
        playerAttack.SetDisableControls(true);
    }

    public void EnablePlayerControls()
    {
        DisableControls = false;
        playerPickUp.DisableControls = false;
        playerAttack.SetDisableControls(false);
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

    
}
