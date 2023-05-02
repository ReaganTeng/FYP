using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    float timer; 

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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

            if (timer >= .7f)
            {
                Destroy(gameObject);
            }
        }
    }
}
