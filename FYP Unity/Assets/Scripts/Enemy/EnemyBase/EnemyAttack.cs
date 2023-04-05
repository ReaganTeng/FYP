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

    // Start is called before the first frame update
    void Start()
    {
        Attackcdtimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Attackcdtimer > 0)
            Attackcdtimer -= Time.deltaTime;

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
        if (Attackcdtimer < AttackCD / 2)
        {
            animator.SetBool("attack", false);
        }
        else
        {
            player.GetComponent<BoxCollider>().enabled = true;
        }

        //if (Attackcdtimer > AttackCD/2)
        //{
        //    animator.SetBool("attack", false);
        //}
    }

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if (other.CompareTag("Player") /*&& Attackcdtimer <= 0*/)
        {
           
            other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);
            other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            
            animator.SetBool("attack", true);
            //other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);

            other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);

           other.GetComponent<Rigidbody>().AddForce(
                (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 50.0f,
                ForceMode.Impulse
                );
            other.GetComponent<BoxCollider>().enabled = false;

            Attackcdtimer = AttackCD;
        }
        //else
        //if attack hitbox did not collide with player
        //else if (other.tag != "Player"
        //    && Attackcdtimer > 0 && Attackcdtimer <= AttackCD)
        //{
        //    Debug.Log("COLLIDER TRUE");
        //    player.GetComponent<BoxCollider>().enabled = true;
        //}
    }
}
