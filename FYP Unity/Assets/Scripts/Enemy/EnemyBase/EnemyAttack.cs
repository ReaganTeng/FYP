using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyScript es;
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

    private void OnTriggerStay(Collider other)
    {
        // if it is the player
        if (other.CompareTag("Player") && Attackcdtimer <= 0)
        {
            other.GetComponent<PlayerStats>().ChangeHealth(-es.AttackDamage);
            Attackcdtimer = AttackCD;
        }
    }
}
