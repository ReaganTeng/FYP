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
        //playerBoxCollider = GameObject.FindGameObjectWithTag("playerboxcollider");
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerBoxCollider = GameObject.FindGameObjectWithTag("playerboxcollider");

        if (Attackcdtimer > 0)
        {
            Attackcdtimer -= Time.deltaTime;
            //player.GetComponent<PlayerMovement>().setAnimator(true);

            if (Attackcdtimer < AttackCD / 2)
            {
                animator.SetBool("attack", false);
                transform.parent.GetComponent<BoxCollider>().enabled = true;

            }
        }
        //else
        //{
        //    //player.GetComponent<PlayerMovement>().setAnimator(false);
        //    //playerBoxCollider.SetActive(true);
        //    //playerBoxCollider.GetComponent<BoxCollider>().enabled = true;
        //    transform.parent.GetComponent<BoxCollider>().enabled = true;
        //    //Debug.Log("Parent name " + GetComponentInParent<Transform>());
        //}
    }

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {

        //player = GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<PlayerMovement>().setAnimator(true);


        // if it is the player
        if ((other.CompareTag("Player") && Attackcdtimer <= 0
            /*|| other.CompareTag("playerboxcollider")*/)
            && GetComponent<BoxCollider>().enabled == true)
        {
            animator.SetBool("attack", true);
            //other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);


            other.GetComponent<PlayerMovement>().setAnimator(true);
            other.GetComponent<PlayerStats>().ResetConsecutiveHit();
            other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);

            //Debug.Log("HIT");

            /*other.GetComponent<Rigidbody>().AddForce(
                 (other.GetComponent<Transform>().position - GetComponentInParent<Transform>().position).normalized * 50.0f,
                 ForceMode.Impulse
                 );*/

            //playerBoxCollider.GetComponent<BoxCollider>().enabled = false;
            transform.parent.GetComponent<BoxCollider>().enabled = false;

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
