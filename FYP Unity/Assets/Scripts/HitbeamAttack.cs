using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitbeamAttack : MonoBehaviour
{
    [SerializeField] float AttackCD;
    float Attackcdtimer;

    void Start()
    {
        Attackcdtimer = 0;
    }
   
    private void Update()
    {
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
            other.GetComponent<PlayerStats>().ChangeHealth(-5);

            //PLAY HURT ANIMATION
            other.GetComponent<PlayerMovement>().setAnimator(true);
            //
            other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
            other.GetComponent<PlayerStats>().resetval();

            Attackcdtimer = AttackCD;
        }
    }
}
