using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float PlayerHealth;
    [SerializeField] float PlayerAttack;

    public void ChangeHealth(float Healthchange)
    {
        PlayerHealth += Healthchange;
        Debug.Log("Player Health: " + PlayerHealth);

        if (PlayerHealth <= 0)
        {
            Debug.Log("Imagine dying to ingredients!!");
        }
    }


    public float GetPlayerAttack()
    {
        return PlayerAttack;
    }
}
