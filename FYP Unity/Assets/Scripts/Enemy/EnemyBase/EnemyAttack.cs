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
     GameObject playerBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        Attackcdtimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        playerBoxCollider = GameObject.FindGameObjectWithTag("playerboxcollider");
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBoxCollider = GameObject.FindGameObjectWithTag("playerboxcollider");

        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;
            player.GetComponent<PlayerMovement>().setAnimator(true);

            if (Attackcdtimer < AttackCD / 2)
            {
                animator.SetBool("attack", false);

            }
        }
        else
        {
            player.GetComponent<PlayerMovement>().setAnimator(false);
            //playerBoxCollider.SetActive(true);
            playerBoxCollider.GetComponent<BoxCollider>().enabled = true;
        }

        
        //player.GetComponent<PlayerMovement>().setAnimator(true);

        //if (Attackcdtimer > AttackCD/2)
        //{
        //    animator.SetBool("attack", false);
        //}
    }

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {

        //player = GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<PlayerMovement>().setAnimator(true);


        // if it is the player
        if ((other.CompareTag("Player") /*&& Attackcdtimer <= 0*/
            || other.CompareTag("playerboxcollider"))
            && GetComponent<BoxCollider>().enabled == true)
        {
            animator.SetBool("attack", true);
            //other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);

            Debug.Log("HIT");

            /*other.GetComponent<PlayerMovement>().setAnimator(true);
            other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);*/

            //PLAY HURT ANIMATION
            player.GetComponent<PlayerMovement>().setAnimator(true);
            //
            player.GetComponent<PlayerStats>().ResetConsecutiveHit();
            player.GetComponent<PlayerStats>().ChangeFervor(-10.0f);

            /*other.GetComponent<Rigidbody>().AddForce(
                 (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 50.0f,
                 ForceMode.Impulse
                 );*/
            //playerBoxCollider.SetActive(false);

            //playerBoxCollider.SetActive(false);
            playerBoxCollider.GetComponent<BoxCollider>().enabled = false;

            Attackcdtimer = AttackCD;
        }
        //if attack hitbox did not collide with player
        //else if (other.tag != "Player"
        //    && Attackcdtimer > 0 && Attackcdtimer <= AttackCD)
        //{
        //    Debug.Log("COLLIDER TRUE");
        //    player.GetComponent<BoxCollider>().enabled = true;
        //}
    }
}
