using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float PlayerHealth;
    [SerializeField] float PlayerAttack;

    //GameObject[] zones;
    //public int zone_no;

    public void ChangeHealth(float Healthchange)
    {
        PlayerHealth += Healthchange;
        Debug.Log("Player Health: " + PlayerHealth);

        if (PlayerHealth <= 0)
        {
            Debug.Log("Imagine dying to ingredients!!");
        }
    }


    /*private void Update()
    {
        zones = GameObject.FindGameObjectsWithTag("Zone");

        for (int i = 0; i < zones.Length; i++)
        {
            if (/gameObject.transform.position.x < zones[i].GetComponent<Transform>().position.x + (zones[i].GetComponent<Transform>().localScale.x / 2)
             && gameObject.transform.position.x > zones[i].GetComponent<Transform>().position.x - (zones[i].GetComponent<Transform>().localScale.x / 2)
             && gameObject.transform.position.z > zones[i].GetComponent<Transform>().position.z - (zones[i].GetComponent<Transform>().localScale.z / 2)
            && gameObject.transform.position.z < zones[i].GetComponent<Transform>().position.z + (zones[i].GetComponent<Transform>().localScale.z / 2)

             )
            {
                zone_no = zones[i].GetComponent<Zone>().zone_number;
                //Debug.Log("PLAYER ZONE " + zone_no);
            }
            
        }
    }*/

    public float GetPlayerAttack()
    {
        return PlayerAttack;
    }
}
