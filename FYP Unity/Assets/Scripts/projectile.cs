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

        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"
            || other.tag == "wall")
        {
            if (other.tag == "Player"
                && other.GetComponent<BoxCollider>().enabled == true)
            {
                other.GetComponent<PlayerMovement>().setAnimator(true);
                other.GetComponent<PlayerStats>().ResetConsecutiveHit();
                other.GetComponent<PlayerStats>().ChangeFervor(-10.0f);
                other.GetComponent<PlayerStats>().resetval();

            }

            if (other.tag == "wall")
            {
                if (timer >= .7f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);

            }
        }
    }
}
