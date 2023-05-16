using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneCheck : MonoBehaviour
{
    int zoneno;
    GameObject[] zone;

   

    public void Start()
    {
        
        //zoneno = 0;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();

        //PlayerHealth = playerProgress.PlayerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();

        //Debug.Log("CURRENT PLAYER ZONE " + zoneno);
    }

    public int getZoneno()
    {
        return zoneno;
    }


    public void BoundaryCheck()
    {
        for (int i = 0; i < zone.Length; i++)
        {
            if (transform.position.x < zone[i].GetComponent<Transform>().position.x + (zone[i].GetComponent<Transform>().lossyScale.x / 2)
             && transform.position.x > zone[i].GetComponent<Transform>().position.x - (zone[i].GetComponent<Transform>().lossyScale.x / 2)
             && transform.position.z > zone[i].GetComponent<Transform>().position.z - (zone[i].GetComponent<Transform>().lossyScale.z / 2)
            && transform.position.z < zone[i].GetComponent<Transform>().position.z + (zone[i].GetComponent<Transform>().lossyScale.z / 2)
             )

            {
                zoneno = zone[i].GetComponent<WhatZone>().zone_number;
                break;
            }
            else
            {
                zoneno = 0;
            }
        }
    }
}
