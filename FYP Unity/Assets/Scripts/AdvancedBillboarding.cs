using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedBillboarding : MonoBehaviour
{
    // Start is called before the first frame update

    public bool xOffset;

    // Update is called once per frame
    void Update()
    {
        if (xOffset)
            transform.rotation = Quaternion.Euler(new Vector3(-90,0,0)) * Quaternion.LookRotation(Camera.main.transform.forward);
        else
            transform.forward = Camera.main.transform.forward;    
    }
}
