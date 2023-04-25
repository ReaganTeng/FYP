using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaserScript : MonoBehaviour
{
    [SerializeField] GameObject lockon;
    GameObject lockonbeam;
    [SerializeField] GameObject hit;
    GameObject hitbeam;
    [SerializeField] GameObject pivotpoint;
    GameObject pivot;

    [SerializeField] NavMeshAgent navMeshAgent;

    GameObject playerGO;
    float chasingspeed;
    EnemyScript.Phases enemyPhase;

    float time_att_1;
    float time_att_2;
    float change_of_attk_type_1;

    Transform starting_location;
    Transform ending_location;
    float dist;
    bool beam_mode;

    EnemyScript enemyScript;

    [SerializeField] GameObject attackhitbox;

    // Start is called before the first frame update
    void Start()
{
        GetComponent<EnemyScript>().set_enemyType(EnemyScript.EnemyType.CHASER);

        lockonbeam = null;
        hitbeam = null;
        pivot = null;

        beam_mode = false;
        time_att_1 = 0;
        time_att_2 = 0;
        change_of_attk_type_1 = 0;

        playerGO = GameObject.FindGameObjectWithTag("Player");

        chasingspeed = 4.0f;
        attackhitbox.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        enemyPhase = GetComponent<EnemyScript>().return_current_phase();
        enemyScript = GetComponent<EnemyScript>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent.speed = chasingspeed;
        navMeshAgent.acceleration = chasingspeed;

        dist = Vector3.Distance(transform.position, playerGO.transform.position);

        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    //case EnemyScript.Phases.ATTACK_TYPE_1:
                    {
                        attackhitbox.GetComponent<BoxCollider>().enabled = true;
                        GetComponent<BoxCollider>().enabled = true;

                        time_att_1 = 0;
                        //if (dist <= 4.0f)
                        //{
                        beam_mode = true;
                        //}
                        //else if (dist > 4.0f
                        //    && beam_mode == false)
                        //{
                        //    enemyScript.set_current_phase(EnemyScript.Phases.ATTACK_TYPE_1);
                        //}

                        if (beam_mode == true)
                        {
                            time_att_2 += 1 * Time.deltaTime;
                            chasingspeed = 0.0f;
                            if (time_att_2 < 1.1f && time_att_2 > 1.0f)
                            {
                                //PLACE LOCK ON BEAM
                                starting_location = transform;
                                ending_location = playerGO.transform;
                                if (lockonbeam == null)
                                {
                                    pivot = Instantiate(pivotpoint,
                                       transform.position,
                                       Quaternion.Euler(0, 0, 0));
                                    pivot.transform.SetParent(transform);

                                    lockonbeam = Instantiate(lockon,
                                        transform.position,
                                        Quaternion.Euler(0, 0, 0));
                                    lockonbeam.transform.localScale =
                                        new Vector3(.1f, 
                                        lockon.transform.localScale.y,
                                        lockon.transform.localScale.z);
                                   
                                    lockonbeam.transform.SetParent(pivot.transform);

                                    pivot.transform.LookAt(
                                        new Vector3(ending_location.position.x, transform.position.y, ending_location.position.z)
                                        );                                   
                                }
                                //
                            }
                            if (time_att_2 > 2.3f && time_att_2 < 2.7f)
                            {
                                if (hitbeam == null)
                                {
                                    hitbeam = Instantiate(hit,
                                       transform.position,
                                       Quaternion.Euler(0, 0, 0));

                                    hitbeam.transform.localScale =
                                        new Vector3(hit.transform.localScale.x, 
                                        hit.transform.localScale.y,
                                        5);

                                    hitbeam.transform.SetParent(pivot.transform);
                                    hitbeam.transform.rotation = pivot.transform.rotation;
                                }
                            }

                            if (time_att_2 > 2.8f)
                            {
                                DestroyBeams();
                                enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                            }
                        }
                        break;
                    }
                case EnemyScript.Phases.ATTACK_TYPE_1:
                    {
                        attackhitbox.GetComponent<BoxCollider>().enabled = true;
                        GetComponent<BoxCollider>().enabled = true;
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);

                        beam_mode = false;

                        //chasingspeed = 2.0f;
                        time_att_2 = 0;
                        time_att_1 += Time.deltaTime;
                        change_of_attk_type_1 += Time.deltaTime;

                        if (change_of_attk_type_1 >= 4.0f)
                        {
                            change_of_attk_type_1 = 0.0f;
                        }

                        //back away during post attack
                        if (GetComponentInChildren<EnemyAttack>().getpostattack()
                            || change_of_attk_type_1 >= 3.0f)
                        {
                            chasingspeed = 2.0f;
                            if (dist <= 5.0f)
                            {
                                navMeshAgent.enabled = false;
                                Vector3 resultingVector = -playerGO.transform.position + transform.position;
                                resultingVector.y = 0;
                                GetComponent<Rigidbody>().velocity = resultingVector;
                            }
                            else
                            {
                                navMeshAgent.enabled = true;
                                GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                                navMeshAgent.SetDestination(playerGO.transform.position);
                            }
                        }
                        //
                        //continue to chase the player
                        else
                        {
                            chasingspeed = dist /** 0.2f*/;
                            navMeshAgent.enabled = true;
                            GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                            navMeshAgent.SetDestination(playerGO.transform.position);
                        }
                        //

                        if (time_att_1 > 20.0f)
                        {
                            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                        }
                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        beam_mode = false;
                        chasingspeed = 0.0f;
                        time_att_2 = 0.0f;
                        time_att_1 = 0.0f;
                        change_of_attk_type_1 = 0.0f;

                        GetComponent<EnemyScript>().cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        beam_mode = false;
                        chasingspeed = 0.0f;
                        time_att_2 = 0.0f;
                        time_att_1 = 0.0f;

                        //if (dist <= 5.0f)
                        //{
                        //    Vector3 resultingVector = -playerGO.transform.position + transform.position;
                        //    resultingVector.y = 0;
                        //    GetComponent<Rigidbody>().velocity = resultingVector;
                        //}

                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            enemyScript.ifUpdatingfalse();
            DestroyBeams();

        }

        enemyScript.steering();
    }


    public void DestroyBeams()
    {
        if (hitbeam != null)
        {
            Destroy(hitbeam);
        }
        if (lockonbeam != null)
        {
            Destroy(lockonbeam);
        }
        if (pivot != null)
        {
            Destroy(pivot);
        }
    }

}
