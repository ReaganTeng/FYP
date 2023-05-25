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
    float timer_2;

    List<int> indexes;
    bool stopupdating;


    int attacked_enemy;

    bool enemies_updating;

    bool found_attacked_enemy;

    // Start is called before the first frame update
    void Start()
    {
        attacked_enemy = -1;

        found_attacked_enemy = false;
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
       
        if (timer_2 > .5f)
        {
                //CHECK IF ANY OF THE ENEMIES ARE UPDATING
                for (int i = 0; i < other_enemies.Length; i++)
                {
                    if (other_enemies[i] != null)
                    {
                        //IF THERE'S AT LEAST ONE ENEMY UPDATING
                        if (other_enemies[i].GetComponent<EnemyScript>().getupdating())
                        {
                            enemies_updating = true;
                            break;
                        }
                        //IF NONE OF THE ENEMIES ARE UPDATING
                        else
                        {
                            enemies_updating = false;
                        }
                    }
                }
                //

                timer += Time.deltaTime;
                if (timer >= .1f
                    && !stopupdating)
                {
                    //LOOK AT ALL ENEMIES
                    for (int i = 0; i < other_enemies.Length; i++)
                    {
                        if (other_enemies[i] != null)
                        {
                            if (other_enemies[i].GetComponent<EnemyScript>().getupdating())
                            {
                                indexes.Add(i);
                            }
                        }
                    }
                    //

                    //CHOOSE A RANDOM ENEMY
                    int range = Random.Range(0, indexes.Count);


                    //IF ENEMY HAS BEEN ATTACKED, SET THAT PARTICULAR ENEMY TO ATTACK MODE
                    if (!found_attacked_enemy)
                    {
                        for (int x = 0; x < indexes.Count; x++)
                        {
                            if (other_enemies[indexes[x]].GetComponent<EnemyScript>().getattacked())
                            {
                                //FOUND THE ENEMY THAT'S ATTACKED
                                other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.COOLDOWN);
                                other_enemies[indexes[x]].GetComponent<EnemyScript>().setattacked(false);
                                found_attacked_enemy = true;
                                attacked_enemy = x;
                                break;
                            }
                            else
                            {
                                other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.AVOID);
                            }
                        }
                    }
                    //


                    //SET THE REST OF THE ATTACKED ENEMY TO ATTACKED = FALSE
                    for(int a = 0; a < indexes.Count; a++)
                    {
                        if (a != attacked_enemy)
                        {
                            other_enemies[indexes[a]].GetComponent<EnemyScript>().setattacked(false);
                            other_enemies[indexes[a]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.AVOID);
                        }
                    }
                    //

                    //IF NO ENEMY HAS BEEN FOUND ATTACKED
                    if (!found_attacked_enemy)
                    {
                        for (int z = 0; z < indexes.Count; z++)
                        {
                            //if landed on chosen enemy
                            if (z == range)
                            {
                                //other_enemies[indexes[x]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.AVOID);
                                other_enemies[indexes[z]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.COOLDOWN);
                               
                            }
                            //
                            //non-selected enemmies stay in about to attackMode
                            else
                            {
                                other_enemies[indexes[z]].GetComponent<EnemyScript>().set_current_phase(EnemyScript.Phases.AVOID);
                            }
                            //
                        }

                        found_attacked_enemy = true;
                    }

                    attacked_enemy = -1;
                    stopupdating = true;
                }
               
        }
        else
        {
            timer_2 += Time.deltaTime;
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
        found_attacked_enemy = boolean;
        other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void recalculate_numberofenemies()
    {
        other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        timer_2 = 0.0f;
    }


    //UNUSED
    /*void position_distribution(int i)
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
            //other_enemies[i].GetComponent<NavMeshAgent>().SetDestination(destinations[destinations.Count - 1]);
        }
        //enemies.GetComponent<EnemyScript>().avoidanceCode(1);
        //Debug.Log("DONE");
    }*/
    
}
