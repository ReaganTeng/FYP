using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
