using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyScript es;
    [SerializeField] float AttackCD;
    float Attackcdtimer;

    [SerializeField] Animator animator;
    GameObject player;

    bool post_attack;

    // Start is called before the first frame update
    void Start()
    {
        Attackcdtimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        post_attack = false;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;

            if (Attackcdtimer < AttackCD / 5)
            {
                transform.parent.GetComponent<BoxCollider>().enabled = true;
            }

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType() 
                == EnemyScript.EnemyType.CHASER)
            {
                if (Attackcdtimer < AttackCD / 2)
                {
                    animator.SetBool("attack", false);
                }
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

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if (other.CompareTag("Player") && Attackcdtimer <= 0
            && GetComponent<BoxCollider>().enabled == true)
        {
            animator.SetBool("attack", true);
            other.GetComponent<PlayerMovement>().setAnimator(true);
            other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);

            if (transform.parent.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.JUMPER)
            {
                other.GetComponent<Rigidbody>().AddForce(
                (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 10.0f,
                ForceMode.Impulse
                );
                transform.parent.GetComponent<BoxCollider>().enabled = false;

            }

            post_attack = true;

            Attackcdtimer = AttackCD;
        }
    }
}
