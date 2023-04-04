using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyScript es;
    [SerializeField] float AttackCD;
    float Attackcdtimer;


    [SerializeField] Animator animator;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Attackcdtimer = 0;
    }

    private void Update()
    {
        //Debug.Log("Time is "+ Attackcdtimer);

        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;

            if (Attackcdtimer < AttackCD / 2)
            {
                animator.SetBool("attack", false);
            }
        }


        //if (Attackcdtimer > AttackCD/2)
        //{
        //    animator.SetBool("attack", false);
        //}

        /*if (player != null)
        {
            if (Attackcdtimer <= 0)
            {
                player.GetComponentInChildren<PlayerMovement>().SetSRColor(Color.green);
            }
            else
            {
                player.GetComponentInChildren<PlayerMovement>().SetSRColor(Color.white);
            }
        }*/
    }

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if (other.CompareTag("Player") && Attackcdtimer <= 0)
        {
            //if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "run")
            //{
                animator.SetBool("attack", true);
                other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            //}

            //other.GetComponent<Rigidbody>().AddForce(
            //    (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 50.0f, 
            //    ForceMode.Impulse
            //    );
            Attackcdtimer = AttackCD;
        }

        /*if (other.CompareTag("Player"))
        {
            animator.SetBool("attack", true);

        }*/

        //else
        //{
        //    Debug.Log("NO COLLISSION");

        //}
    }
}
