using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawgizmos : MonoBehaviour
{
    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        var boxcollider = box;
        Gizmos.color = Color.blue;
        /*Gizmos.DrawWireCube(boxcollider.center + GetComponent<Transform>().position, 
            boxcollider.size - GetComponent<Transform>().localScale);*/
        Gizmos.DrawWireCube(boxcollider.GetComponent<BoxCollider>().center,
            boxcollider.GetComponent<BoxCollider>().size);

    }
}
