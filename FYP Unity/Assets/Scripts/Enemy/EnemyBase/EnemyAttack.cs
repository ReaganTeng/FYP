using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Com.LuisPedroFonseca.ProCamera2D;


public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyScript es;
    float AttackCD;
    float Attackcdtimer;

    [SerializeField] Animator animator;
    GameObject player;

    bool post_attack;

    bool attacking_present;
    bool player_in_hitbox;

    bool attacking;



    GameObject gamemanager;
    // Start is called before the first frame update

    
    void Start()
    {
        player_in_hitbox = false;
        attacking = false;
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        player = GameObject.FindGameObjectWithTag("Player");
        attacking_present = false;
        AttackCD = 0;
        Attackcdtimer = 0;
        post_attack = false;
    }

    private void Update()
    {
        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;
        }

        //TO MINIMISE THE BOXCOLLIDER OF ENEMY AND PLAYER GETTING STUCK TOGETHER FOR JUMPERS
        if (Attackcdtimer < AttackCD / 5)
        {
            transform.parent.transform.parent.GetComponent<BoxCollider>().enabled = true;

        }
        //
    }


    public float getattackCD()
    {
        return AttackCD;
    }

    public void setattackCDtimer(float t)
    {
        Attackcdtimer = AttackCD;
    }
    public float getattackCDtimer()
    {
        return Attackcdtimer;
    }

    public void setattacking_present(bool t)
    {
        attacking_present = t;
    }
    public bool get_attacking_present()
    {
        return attacking_present;
    }

    public void set_attacking(bool t)
    {
        attacking = t;
    }
    public bool get_attacking()
    {
        return attacking;
    }


    public bool playerinhitbox()
    {
        return player_in_hitbox;
    }



    public void setpostattack(bool pa)
    {
        post_attack = pa;
    }
    public bool getpostattack()
    {
        return post_attack;
    }

   
    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        //all decrease by same amount, use upgrade to use
        if (other.CompareTag("Player"))
        {
            if (!post_attack)
            {
                player_in_hitbox = true;
            }
        }

        // if it is the player
            if (other.CompareTag("Player")
                        && other.GetComponentInChildren<Animator>().GetNextAnimatorStateInfo(0).IsName("Dash") == false
            && Attackcdtimer <= 0
            && GetComponent<BoxCollider>().enabled == true
            )
        {
            attacking = true;

            //IF ENEMY IS A JUMPER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.JUMPER             
                && transform.parent.transform.parent.GetComponent<EnemyScript>().return_current_phase() != EnemyScript.Phases.AVOID)
            {
                other.GetComponent<PlayerMovement>().setHurtAnimation(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                //other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
                //THICK SKIN POWERUP
                other.GetComponent<PlayerStats>().ChangeFervor(-15.0f * other.GetComponent<PlayerStats>().getpp().return_thick_skin());
                //
                other.GetComponent<PlayerStats>().resetval();
                ProCamera2DShake.Instance.ShakeUsingPreset("DamageShake");
            }
            //

            //IF ENEMY IS A CHASER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.CHASER)
            {
                if (!post_attack)
                {
                    attacking_present = true;

                    //Attackcdtimer = AttackCD;
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                    {
                        transform.parent.transform.parent.GetComponent<NavMeshAgent>().enabled = false;
                        transform.parent.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        animator.SetBool("about2attack", true);
                    }
                }

                //if (animator.GetBool("attack"))
                //{
                //    //Debug.Log("ATTACK TRUE");
                //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack")
                //        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .2f)
                //    {
                //        attacking_present = true;
                //        Attackcdtimer = AttackCD;
                //        //Debug.Log("HIT");
                //        //
                //    }
                //}
                //else
                //{
                //    animator.SetBool("about2attack", true);
                //}
            }
            //

            //IF ENEMY IS A CHARGER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.CHARGER
                && transform.parent.transform.parent.GetComponent<EnemyScript>().return_current_phase() != EnemyScript.Phases.AVOID
                )
            {
                other.GetComponent<PlayerMovement>().setHurtAnimation(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                //THICK SKIN POWERUP
                other.GetComponent<PlayerStats>().ChangeFervor(-15.0f * other.GetComponent<PlayerStats>().getpp().return_thick_skin());
                //
                other.GetComponent<PlayerStats>().resetval();
                ProCamera2DShake.Instance.ShakeUsingPreset("DamageShake");

                Attackcdtimer = AttackCD;
            }
            //

            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.JUMPER)
            {
                other.GetComponent<Rigidbody>().AddForce(
                (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 10.0f,
                ForceMode.Impulse
                );

                transform.parent.transform.parent.GetComponent<BoxCollider>().enabled = false;
                Attackcdtimer = AttackCD;
            }

           
           AttackCD = 1.0f;
           
        }
        
    }



   

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            player_in_hitbox = false;

            //Debug.Log("PLAYER EXIT HITBOX");

            //attacking_present = false;
            //delayTime = 0.0f;
            //AttackCD = 0;
            //attacks_performed = 0;
            //Attackcdtimer = 0;
            
            //if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
            //               == EnemyScript.EnemyType.CHASER)
            //{
            //    animator.SetBool("about2attack", false);
            //    animator.SetBool("attack", false);
            //}
        }
    }

}
