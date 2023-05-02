using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyScript es;
   /* [SerializeField]*/ float AttackCD;
    float Attackcdtimer;

    [SerializeField] Animator animator;
    GameObject player;

    bool post_attack;


    [SerializeField] AnimationClip attackclip;
    [SerializeField] AnimationClip about2atkclip;

    //determine how many times the player can attack at once
    [SerializeField] int attacks_per_session;

    int attacks_performed;
    bool attacking_present;

    float delayTime;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attacking_present = false;
        delayTime = 0.0f;
        AttackCD = 0;
        attacks_performed = 0;
        Attackcdtimer = 0;
        post_attack = false;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;
        }


        if (Attackcdtimer < AttackCD / 5)
        {
            transform.parent.transform.parent.GetComponent<BoxCollider>().enabled = true;
        }

        if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
            == EnemyScript.EnemyType.CHASER)
        {
            /*&& delayTime
            >= transform.parent.transform.parent.GetComponent<EnemyScript>().getCurrentAnimationLength() *
            attacks_performed*/

            if (Attackcdtimer < AttackCD / 5)
            {
                if (attacks_performed < attacks_per_session)
                {
                    player.GetComponentInChildren<Animator>().SetBool("Hurt", false);
                }
            }

            //if (Attackcdtimer <= 0
            //    && attacks_performed < attacks_per_session)
            //{
            //    Attackcdtimer = AttackCD;
            //}

            //DELAY FOR CHASER
            if (animator.GetBool("about2attack")
                && animator.GetCurrentAnimatorStateInfo(0).IsName("aboutattack"))
            {
                //Debug.Log("ABOUT 2 ATTACK");
                delayTime += Time.deltaTime;
                if (delayTime
                    >= /*transform.parent.transform.parent.GetComponent<EnemyScript>().getCurrentAnimationLength()*/
                    animator.GetCurrentAnimatorStateInfo(0).length)
                {
                    animator.SetBool("attack", true);
                }
            }
            //

            //ADD 1 ATTACK TO EACH LOOP
            if (attacking_present)
            {
                attacks_performed += 1;
                attacking_present = false;
            }
            //

            //END THE LOOP WHEN ATTACKS PERFORM EXCEEDED
            if (attacks_performed >= attacks_per_session)
            {
                animator.SetBool("attack", false);
                animator.SetBool("about2attack", false);
                //animator.SetBool("chasingPlayer", false);
                post_attack = true;
                attacks_performed = 0;
                delayTime = 0.0f;
            }
            //

            

        }

    }


    public void setpostattack(bool pa)
    {
        post_attack = pa;
    }
    public bool getpostattack()
    {
        return post_attack;
    }

    public bool return_whether_back_away()
    {
        if (Attackcdtimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if (other.CompareTag("Player")
            && Attackcdtimer <= 0
            && GetComponent<BoxCollider>().enabled == true
            && attacks_performed < attacks_per_session)
        {
            
            //IF ENEMY IS A JUMPER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.JUMPER)
            {
                other.GetComponent<PlayerMovement>().setAnimator(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
            }
            //

            //IF ENEMY IS A CHASER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.CHASER)
            {
                if (animator.GetBool("attack"))
                {
                    //Debug.Log("ATTACK TRUE");
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                    {
                        //HIT PLAYER EVERY TIME EACH LOOP ENDS
                        other.GetComponent<PlayerMovement>().setAnimator(true);
                        other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                        other.GetComponent<PlayerStats>().ChangeFervor(-5.0f);
                        attacking_present = true;
                        Attackcdtimer = AttackCD;
                        Debug.Log("HIT");
                        //
                    }
                }
                else
                {
                    animator.SetBool("about2attack", true);
                }
            }
            //

            //IF ENEMY IS A CHARGER
            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                == EnemyScript.EnemyType.CHARGER
                )
            {
                other.GetComponent<PlayerMovement>().setAnimator(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                other.GetComponent<PlayerStats>().ChangeFervor(-15.0f);
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

            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHARGER)
            {
                AttackCD = attackclip.length;
            }
            else
            {
                AttackCD = 1.0f;
            }
        }

           

        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking_present = false;
            delayTime = 0.0f;
            AttackCD = 0;
            attacks_performed = 0;
            Attackcdtimer = 0;


           

            if (transform.parent.transform.parent.GetComponent<EnemyScript>().return_enemyType()
                           == EnemyScript.EnemyType.CHASER)
            {
                animator.SetBool("about2attack", false);
                animator.SetBool("attack", false);
            }
        }
    }

}
