using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovement : MonoBehaviour
{
    public GameObject objectToEnable1;
    public GameObject objectToEnable2;
    public GameObject objectToEnable3;

    private Vector3 targetPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            transform.position = targetPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnableObject1"))
        {
            objectToEnable1.SetActive(true);
        }
        else if (other.gameObject.CompareTag("EnableObject2"))
        {
            objectToEnable2.SetActive(true);
        }
        else if (other.gameObject.CompareTag("EnableObject3"))
        {
            objectToEnable3.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnableObject1"))
        {
            objectToEnable1.SetActive(false);
        }
        else if (other.gameObject.CompareTag("EnableObject2"))
        {
            objectToEnable2.SetActive(false);
        }
        else if (other.gameObject.CompareTag("EnableObject3"))
        {
            objectToEnable3.SetActive(false);
        }
    }
}
