using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float EnemyHealth;
    public float AttackDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Floor"))
            return;

        // If its from player attack
        if (other.CompareTag("Attack"))
        {
            EnemyHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();

            Debug.Log("Enemy Health Left: " + EnemyHealth);
            // Precise Kill
            if (EnemyHealth == 0)
            {
                Debug.Log("Precise Kill!");
                Destroy(gameObject);
            }

            else if (EnemyHealth < 0)
            {
                Debug.Log("Killed!");
                Destroy(gameObject);
            }
        }
    }
}
