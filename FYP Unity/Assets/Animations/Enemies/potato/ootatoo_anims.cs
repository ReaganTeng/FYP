using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ootatoo_anims : MonoBehaviour

    
{
    public GameObject ootatoo;
    // Start is called before the first frame update
    void Start()
    {
        ootatoo.GetComponent<Animator>().Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
