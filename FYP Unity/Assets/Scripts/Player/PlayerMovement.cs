using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;



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

    bool shiftPressed = false;

    // Minimap
    GameObject minimapCanvas;

  
    Animator mAnimator;

    [SerializeField] GameObject dashtimer_slider;

    private void Start()
    {
       

        dashcdtimer = 0.0f;
        delaytime = 0.0f;
        isWalking = false;
        DisableControls = false;
        minimapCanvas = GameObject.FindGameObjectWithTag("MinimapCanvas");
        minimapCanvas.SetActive(false);


        mAnimator = GetComponentInChildren<Animator>();

        if (dashtimer_slider != null)
        {
            dashtimer_slider.GetComponent<Slider>().maxValue = DashCD;
            dashtimer_slider.GetComponent<Slider>().minValue = 0;
        }
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

        //for the dash timer bar
        if (dashcdtimer > 0)
        {
            dashcdtimer -= Time.deltaTime;
            if (dashtimer_slider != null)
            {
                dashtimer_slider.SetActive(true);
                dashtimer_slider.GetComponent<Slider>().value = dashcdtimer;
            }
        }
        else
        {
            if (dashtimer_slider != null)
            {
                dashtimer_slider.SetActive(false);
            }
        }
        //

        if (!mAnimator.GetCurrentAnimatorStateInfo(0).IsName("hurt_knife")
            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("hurt_pin")
            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("hurt_spatula")
                         //&& notheavyattackingordashing()
                         && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")

             && !playerAttack.attacking_or_not()
             )
        {
            mAnimator.SetBool("walking", isWalking);
            mAnimator.SetBool("WalkFront", walkFront);
            mAnimator.SetBool("WalkBack", walkBack);
        }
        else
        {
            mAnimator.SetBool("walking", false);
            mAnimator.SetBool("WalkFront", false);
            mAnimator.SetBool("WalkBack", false);
        }


        //BACKWALK
        {
            if (Input.GetKey(KeyCode.W)
                    && !Input.GetKey(KeyCode.S)
                    && !Input.GetKey(KeyCode.A)
                    && !Input.GetKey(KeyCode.D)
                    && !Input.GetKeyDown(KeyCode.LeftShift)
                            //&& notheavyattackingordashing()
                            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")

                && !playerAttack.attacking_or_not()
                )
            {
                walkBack = true;
            }
            else
            {
                walkBack = false;
            }
        }
        //
        //FRONT WALK
        {
            if (!Input.GetKey(KeyCode.W)
                    && Input.GetKey(KeyCode.S)
                    && !Input.GetKey(KeyCode.A)
                    && !Input.GetKey(KeyCode.D)
                    && !Input.GetKeyDown(KeyCode.LeftShift)
                            //&& notheavyattackingordashing()
                            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")

                    && !playerAttack.attacking_or_not()
                )
            {
                walkFront = true;
            }
            else
            {
                walkFront = false;
            }
        }
        //

        if (Input.GetKey(KeyCode.A)
             //&& notheavyattackingordashing()
            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
            && !playerAttack.attacking_or_not()
            )
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.D)
                         //&& notheavyattackingordashing()
                         && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
            //&& notheavyattackingordashing()
            && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash")

             && !playerAttack.attacking_or_not()
             )
        {
            spriteRenderer.flipX = true;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            && !Input.GetKeyDown(KeyCode.LeftShift)
            && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
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

        if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
            && mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)

        

       



        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash")
            && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            mAnimator.SetBool("Dash", false);
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashcdtimer <= 0 && !DisableControls)
        {
            shiftPressed = true;
        }

        // dashing
        if (shiftPressed)
        {
            playerRB.AddForce((orientation.forward * Forwardrun + orientation.right * Rightrun) * PlayerSpeed * DashBy);
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
            iframetimer = IFrame;
            IFrameStart = true;
            mAnimator.SetBool("Dash", true);
            dashcdtimer = DashCD;
            GameSoundManager.PlaySound("Dash");
            //Debug.Log("DASH");
            shiftPressed = false;
        }
        //
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //if hurt animation is playing 
        /*if(mAnimator.GetBool("Hurt"))
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
            && mAnimator.GetBool("click") == false
            )
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // If user is pressing any movement keys
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            )
            {
                if (!playerAttack.attacking_or_not()
                                /*notheavyattackingordashing()*/
                                && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
)
                {
                    playerRB.AddForce((orientation.forward * verticalInput + orientation.right * horizontalInput) * PlayerSpeed);
                    CheckDirection(ref Forwardrun, ref Rightrun, horizontalInput, verticalInput);
                    playerAttack.resetattacking();
                }
            }

            

        }

      
    }



    public bool notheavyattackingordashing()
    {
        if(!mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash")
        /*&& !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_knife")
        && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_pin")
        && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_spatula")*/
        && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_knife")
        && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin")
        && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin"))
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
       mAnimator.SetBool("Hurt", hurt);
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
