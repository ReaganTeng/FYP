using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    float  time;
    private int x_position, z_position;

    //maximum amount of enemies allowed
    [SerializeField] int max_enemies;
    //maximum amount of enemies spawned at once
    [SerializeField] int enemies_per_spawn;
    //spawn enemies once every how many seconds
    [SerializeField] float interval;
    private bool enable = true;

    GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable == false)
        {
            return;
        }

        if (transform.childCount < max_enemies)
        {
            time += 1 * Time.deltaTime;
        }
        else
        {
            time = 0.0f;
        }

        if (time >= interval && transform.childCount < max_enemies)
        {
            for (int i = 0; i < enemies_per_spawn; i++)
            {
                SpawnEnemy();
                gamemanager.GetComponent<EnemyManager>().setupdating(false);
            }

            time = 0;
        }
    }

    public void resetSpawnerTimer(float negativenum, bool set20)
    {
        if (set20)
        {
            time = 0.0f;
        }
        else
        {
            time = 0.0f;
            time -= negativenum;
        }
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

    public GameObject SpawnEnemy(int presetHealth = -1)
    {
        //x_position = Random.Range(-5, 5);
        //z_position = Random.Range(-5, 5);

        GameObject enemyObject = Instantiate(enemy
            , transform.position /*+ new Vector3(x_position * enemy.transform.localScale.x, 0, z_position * enemy.transform.localScale.z)*/
            , transform.rotation);
        enemyObject.transform.SetParent(transform);
        enemyObject.GetComponent<EnemyScript>().setparent(transform);
        if (presetHealth != -1)
            enemyObject.GetComponent<EnemyScript>().SetEnemyHealth(presetHealth);

        return enemyObject;
    }
}
