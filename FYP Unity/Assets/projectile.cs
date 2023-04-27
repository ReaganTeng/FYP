using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"
            || other.tag == "wall")
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerMovement>().setAnimator(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
            }

            Destroy(gameObject);
        }
    }
}
