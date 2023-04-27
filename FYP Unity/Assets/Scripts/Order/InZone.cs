using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InZone : MonoBehaviour
{
    bool PlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = false;
    }

    public bool GetIsPlayerInZone()
    {
        return PlayerInZone;
    }
}
