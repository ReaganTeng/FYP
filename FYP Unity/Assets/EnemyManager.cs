using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameObject[] other_enemies;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemies in other_enemies)
        {
            if (enemies.GetComponent<EnemyScript>().getupdating()
               )
            {
                for (int i = 0; i < other_enemies.Length - 2; i++)
                {
                    if (enemies.GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHASER
                    && enemies.GetComponent<EnemyScript>().return_attackptn() != EnemyScript.AttackPattern.PATTERN_3)
                    {
                        if (enemies.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack")
                            || enemies.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2attack"))
                        {
                            if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack")
                                || GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2attack"))
                            {
                                //Debug.Log("AVOID");
                                enemies.GetComponent<EnemyScript>().avoidanceCode(1);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
