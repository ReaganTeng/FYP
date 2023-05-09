using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    GameObject[] other_enemies;
    GameObject player;

    List<Vector3> destinations;
    //GameObject[] other_enemies;

    float timer;


    List<int> indexes;
    bool stopupdating;

    bool enemies_updating;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        stopupdating = false;

        enemies_updating = false;

        indexes = new List<int>();
        destinations = new List<Vector3>();

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (timer >= abouttoattack_period)
        {
            //attack_type = Random.Range(1, 3);
            if (atkPattern == AttackPattern.PATTERN_1)
            {
                phase = Phases.ATTACK_TYPE_1;
            }
            else if (atkPattern == AttackPattern.PATTERN_2)
            {
                phase = Phases.ATTACK_TYPE_2;
            }
            else if (atkPattern == AttackPattern.PATTERN_3)
            {
                phase = Phases.ATTACK_TYPE_3;
            }
            timer = 0.0f;
        }*/

        

        //if(enemies_updating)
        //{ 
        //    Debug.Log("SOME UPDATING");
        //}
        //else
        //{
        //    Debug.Log("ALL NOT UPDATING");
        //}

        player = GameObject.FindGameObjectWithTag("Player");
        other_enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (other_enemies != null)
        {
            for (int i = 0; i < other_enemies.Length; i++)
            {
                if (other_enemies[i].GetComponent<EnemyScript>().getupdating())
                {
                    enemies_updating = true;
                    break;
                }
                else
                {
                    enemies_updating = false;
                }
            }

            foreach (GameObject enemies in other_enemies)
            {
                if (enemies.GetComponent<EnemyScript>().getupdating())
                {
                    timer += Time.deltaTime;
                    if (timer >= .1f
                        && !stopupdating)
                    {
                        //look at all enemies
                        for (int i = 0; i < other_enemies.Length; i++)
                        {
                            if (/*other_enemies[i].GetComponent<EnemyScript>().return_enemyType() == EnemyScript.EnemyType.CHASER
                            && other_enemies[i].GetComponent<EnemyScript>().return_attackptn() != EnemyScript.AttackPattern.PATTERN_3
                            && other_enemies[i].GetComponent<EnemyScript>().return_current_phase() == EnemyScript.Phases.COOLDOWN
                                &&*/ other_enemies[i].GetComponent<EnemyScript>().getupdating())
                            {
                                indexes.Add(i);
                            }
                        }
                        //

                        //choose a random enemy
                        int range = Random.Range(0, indexes.Count);


                        //Debug.Log("RANGE " + range);
                        for (int x = 0; x < indexes.Count; x++)
                        {
                            //if landed on chosen enemy
                            if (x == range)
                            {
                                other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.COOLDOWN);
                                {
                                    /*if (other_enemies[indexes[x]].GetComponent<EnemyScript>().return_attackptn()
                                        == EnemyScript.AttackPattern.PATTERN_1)
                                    {
                                        other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.ATTACK_TYPE_1);
                                    }
                                    else if (other_enemies[indexes[x]].GetComponent<EnemyScript>().return_attackptn()
                                    == EnemyScript.AttackPattern.PATTERN_2)
                                    {
                                        other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.ATTACK_TYPE_2);
                                    }
                                    else if (other_enemies[indexes[x]].GetComponent<EnemyScript>().return_attackptn()
                                   == EnemyScript.AttackPattern.PATTERN_3)
                                    {
                                        other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.ATTACK_TYPE_3);
                                    }*/
                                }
                            }
                            //
                            //non-selected enemmies stay in about to attackMode
                            else
                            {

                                other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.AVOID);
                            }
                            //
                        }
                        //
                        //Debug.Log("UPDATE");
                        stopupdating = true;
                    }
                }
            }
        }

        }

    public bool getEnemy_update()
    {
        return enemies_updating;
    }
    public void setupdating(bool boolean)
    {
        indexes.Clear();
        timer = 0;
        stopupdating = boolean;
        //Debug.Log("RESET");
    }
    void position_distribution(int i)
    {
        float rand_x = Random.Range(-10, 10);
        float rand_z = Random.Range(-10, 10);

        if (destinations.Count == 0)
        {
            destinations.Add(
                new Vector3(player.transform.position.x + rand_x,
                other_enemies[i].transform.position.y,
                player.transform.position.z + rand_z)
                );
            other_enemies[i].GetComponent<NavMeshAgent>().SetDestination(destinations[0]);
        }
        else
        {
            for (int x = 0; x < destinations.Count;)
            {
                if (new Vector3(rand_x, other_enemies[i].transform.position.y, rand_z)
                == destinations[x])
                {
                    x++;
                }
                else
                {
                    rand_x = Random.Range(-10, 10);
                    rand_z = Random.Range(-10, 10);
                    x = 0;
                }

                if (x == destinations.Count - 1)
                {
                    destinations.Add(
                    new Vector3(player.transform.position.x + rand_x,
                    other_enemies[i].transform.position.y,
                    player.transform.position.z + rand_z)
                    );
                }
            }
            //Debug.Log("POSITION " + /*destinations[*/destinations.Count/* - 1]*/);
            //other_enemies[i].GetComponent<NavMeshAgent>().SetDestination(destinations[destinations.Count - 1]);
        }
        //enemies.GetComponent<EnemyScript>().avoidanceCode(1);
        //Debug.Log("DONE");
    }
    
}
