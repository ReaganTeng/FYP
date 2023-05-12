using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitbeamAttack : MonoBehaviour
{
    [SerializeField] float AttackCD;
    float Attackcdtimer;

    GameObject player;

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
    }

    //change collider size if want to increase attack range
    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if ((other.CompareTag("Player") /*|| other.CompareTag("playerboxcollider")*/)
            && Attackcdtimer <= 0)
        {
            //Debug.Log("PLAYER ATTACKED");
            player.GetComponent<PlayerStats>().ChangeHealth(-5);

            //PLAY HURT ANIMATION
            player.GetComponent<PlayerMovement>().setAnimator(true);
            //
            player.GetComponent<PlayerStats>().ResetConsecutiveHit();
            player.GetComponent<PlayerStats>().ChangeFervor(-10.0f);

            Attackcdtimer = AttackCD;
        }
    }
}
