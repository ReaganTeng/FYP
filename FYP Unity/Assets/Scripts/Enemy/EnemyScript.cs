using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float EnemyHealth;


    private void OnTriggerEnter(Collider other)
    {
        // If its from player attack
        if (other == GameObject.FindGameObjectWithTag("Attack").GetComponent<Collider>())
        {
            EnemyHealth -= 10;

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
