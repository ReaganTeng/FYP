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

    //determine how many times the player can attack at once
    [SerializeField] int attacks_per_session;

    int attacks_performed;
    bool attacking_present;


    // Start is called before the first frame update
    void Start()
    {
        AttackCD = 0;
        attacks_performed = 0;
        Attackcdtimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        post_attack = false;
        attacking_present = false;
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
            
            transform.parent.GetComponent<BoxCollider>().enabled = true;
        }


        if (transform.parent.GetComponent<EnemyScript>().return_enemyType()
            == EnemyScript.EnemyType.CHASER)
        {
            if (attacking_present)
            {
                attacks_performed += 1;
                //Debug.Log("ATTACKS PERFORMED " + attacks_performed);
                attacking_present = false;
            }

            if (Attackcdtimer < AttackCD / 5)
            {
                if (attacks_performed < attacks_per_session)
                {
                    player.GetComponentInChildren<Animator>().SetBool("Hurt", false);
                }
            }

            //if (Attackcdtimer <= 0)
            //{
            //    //stop enemy attack animation
            //    animator.SetBool("attack", false);
            //}

            if (attacks_performed >= attacks_per_session)
            {
                post_attack = true;
                attacks_performed = 0;
            }

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

            other.GetComponent<PlayerMovement>().setAnimator(true);
            other.GetComponent<PlayerStats>().ResetConsecutiveHit();

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.JUMPER)
            {
                other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
            }
            else if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHASER)
            {
                other.GetComponent<PlayerStats>().ChangeFervor(-5.0f);

            }
            else if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHARGER)
            {
                other.GetComponent<PlayerStats>().ChangeFervor(-15.0f);
            }

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.JUMPER)
            {
                other.GetComponent<Rigidbody>().AddForce(
                (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 10.0f,
                ForceMode.Impulse
                );

                transform.parent.GetComponent<BoxCollider>().enabled = false;
            }

            /*if (attacks_performed < attacks_per_session)
            {
                attacks_performed += 1
            }
            else*/
            //if (attacks_performed >= attacks_per_session)
            //{
            //    post_attack = true;
            //attacks_performed = 0;
            //}

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHARGER)
            {
                AttackCD = attackclip.length;
            }
            else
            {
                AttackCD = 1.0f;
            }
            Attackcdtimer = AttackCD;

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType()
              == EnemyScript.EnemyType.CHASER)
            {
                animator.SetBool("attack", true);
                attacking_present = true;
            }

            Debug.Log("HIT");
        }
    }
}
