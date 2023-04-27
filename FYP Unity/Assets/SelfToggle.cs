using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfToggle : MonoBehaviour
{
    public GameObject objectToToggle;
    private bool isColliding = false;

    private void Update()
    {
        if (isColliding)
        {
            objectToToggle.SetActive(true);
        }
        else
        {
            objectToToggle.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Change "Player" to the appropriate tag for the collider you want to detect
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Change "Player" to the appropriate tag for the collider you want to detect
        {
            isColliding = false;
        }
    }
}


