using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitbeamAttack : MonoBehaviour
{
    [SerializeField] float AttackCD;
    float Attackcdtimer;

    // Start is called before the first frame update
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
        if (other.CompareTag("Player") && Attackcdtimer <= 0)
        {
            //Debug.Log("PLAYER ATTACKED");
            other.GetComponent<PlayerStats>().ChangeHealth(-5);
            Attackcdtimer = AttackCD;
        }
    }
}
