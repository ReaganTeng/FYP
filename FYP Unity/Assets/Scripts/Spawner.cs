using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemy;
    private float interval, time;
    private int x_position, z_position, max_enemies;
    private bool enable;
    

    // Start is called before the first frame update
    void Start()
    {
        interval = 0;
        time = 2.0f;
        max_enemies = 5;
        enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*Instantiate(GO,
                    ((ending_location.position - starting_location.position).normalized * (dist / (i + 1))) + transform.position,
                    transform.rotation
                    );*/

        if (enable == false)
        {
            return;
        }
        
            interval += 1 * Time.deltaTime;
            if (interval >= time
                && transform.childCount < max_enemies)
            {
                for (int i = 0; i < 2; i++)
                {
                    x_position = Random.Range(-5, 5);
                    z_position = Random.Range(-5, 5);

                    GameObject enemyObject = Instantiate(enemy,
                               transform.position + new Vector3(x_position, 2, z_position),
                               transform.rotation
                               );
                    enemyObject.transform.SetParent(transform);
                }

                interval = 0;
            }
            else if (transform.childCount >= 5)
            {
                interval = 0;
            }

            //Debug.Log("AMOUNT OF CHILDREN " + transform.childCount);



            /*if(interval_to_destroy >= 10.0f)
            {
                Transform[] children = GetComponentsInChildren<Transform>();
                for (int i = 0; i < 2; i++)
                {
                    children[i].parent = null;
                }
                interval_to_destroy = 0; 
            }

            //if enemy helath == 0, then use this code
            GameObject roguechild = GameObject.FindGameObjectWithTag("Enemy");
            if (roguechild != null)
            {
                if (roguechild.transform.parent == null)
                {
                    Destroy(roguechild);
                }
            }*/
        
    }

    public void SetEnable(bool set)
    {
        enabled = set;
    }

    public bool GetEnable()
    {
        return enabled;
    }

    public void SetMaxEnemies(int enemies)
    {
        max_enemies = enemies;
    }

    public int GetMaxEnemies()
    {
        return max_enemies;
    }


    public void SetTime(float timer)
    {
        time = timer;
    }

    public float GetTime()
    {
        return time;
    }

}
