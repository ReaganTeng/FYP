using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    float  time;
    private int x_position, z_position;
    [SerializeField] int max_enemies;
    [SerializeField] float interval;
    private bool enable;
    

    

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        enable = true;
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
        
            time += 1 * Time.deltaTime;
            if (time >= interval
                && transform.childCount < max_enemies)
            {
                //for (int i = 0; i < 2; i++)
                //{
                    x_position = Random.Range(-5, 5);
                    z_position = Random.Range(-5, 5);

                    GameObject enemyObject = Instantiate(enemy,
                               transform.position + new Vector3(x_position * enemy.transform.localScale.x, 
                               0, z_position * enemy.transform.localScale.z),
                               transform.rotation
                               );
                    enemyObject.transform.SetParent(transform);
                //}

               time = 0;
            }
            else if (transform.childCount >= 2)
            {
                time = 0;
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


    public void SetInterval(float timer)
    {
        interval = timer;
    }

    public float GetInterval()
    {
        return interval;
    }

}
