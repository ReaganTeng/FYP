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

    float rand_z;

    EnemyScript enemyScript;

    [SerializeField] GameObject attackhitbox;

    GameObject gamemanager;


    // Start is called before the first frame update
    void Start()
    { 
        GetComponent<EnemyScript>().set_enemyType(EnemyScript.EnemyType.CHASER);

        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        rand_z = 0;
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

        navMeshAgent.speed = chasingspeed;
        navMeshAgent.acceleration = chasingspeed;

        dist = Vector3.Distance(transform.position, playerGO.transform.position);

        float hitbeamsize = 15;
        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    //case EnemyScript.Phases.ATTACK_TYPE_1:
                    {
                        attackhitbox.GetComponent<BoxCollider>().enabled = false;
                        GetComponent<BoxCollider>().enabled = true;

                        time_att_1 = 0;
                        if (dist <= 2.0f)
                        {
                            beam_mode = true;
                        }
                        //chase the player
                        else if (dist > 2.0f
                            && beam_mode == false)
                        {
                            //enemyScript.set_current_phase(EnemyScript.Phases.ATTACK_TYPE_1);
                            rand_z = Random.Range(-4, 4);
                            navMeshAgent.enabled = true;
                            //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                            navMeshAgent.SetDestination(playerGO.transform.position);
                            GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);

                            chasingspeed = 2.0f;
                        }

                        if (beam_mode == true)
                        {
                            GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);

                            navMeshAgent.enabled = false;
                            time_att_2 += Time.deltaTime;
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
                                    //pivot.transform.SetParent(transform);
                                    pivot.transform.SetParent(
                                        GetComponentInChildren<SpriteRenderer>().transform);

                                    lockonbeam = Instantiate(lockon,
                                        transform.position,
                                        Quaternion.Euler(0, 0, 0));
                                    lockonbeam.transform.localScale =
                                        new Vector3(.05f, 
                                        lockon.transform.localScale.y,
                                        hitbeamsize/10);
                                   
                                    lockonbeam.transform.SetParent(pivot.transform);

                                    pivot.transform.LookAt(
                                        new Vector3(ending_location.position.x, transform.position.y, ending_location.position.z));
                                    //pivot.transform.ro
                                }
                                //
                            }

                            if (time_att_2 < 1.9f
                                && time_att_2 >= 1.1f)
                            {
                                pivot.transform.LookAt(
                                        new Vector3(ending_location.position.x, transform.position.y, ending_location.position.z));
                            }

                            if (time_att_2 >= 1.9f)
                            {
                                GetComponentInChildren<SpriteRenderer>().color = Color.red;
                            }

                            if (time_att_2 > 2.3f && time_att_2 < 2.7f)
                            {
                                GetComponentInChildren<SpriteRenderer>().color = Color.white;

                                //PLACE HIT BEAM
                                GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
                                GetComponentInChildren<Animator>().SetBool("attack", true);

                                if (hitbeam == null)
                                {
                                    hitbeam = Instantiate(hit,
                                       transform.position,
                                       Quaternion.Euler(0, 0, 0));

                                    hitbeam.transform.localScale =
                                        new Vector3(hit.transform.localScale.x, 
                                        hit.transform.localScale.y,
                                        hitbeamsize);

                                    hitbeam.transform.SetParent(pivot.transform);
                                    hitbeam.transform.rotation = /*Quaternion.Euler(0,*/ pivot.transform.rotation/*.y, 0)*/;
                                    //hitbeam.GetComponentInChildren<SpriteRenderer>().transform.rotation =
                                    //    Quaternion.Euler(0, 90, 0);
                                }
                                //
                            }

                            if (time_att_2 > 2.8f)
                            {
                                navMeshAgent.enabled = true;

                                GetComponentInChildren<Animator>().SetBool("attack", false);
                                GetComponentInChildren<SpriteRenderer>().color = Color.white;

                                gamemanager.GetComponent<EnemyManager>().setupdating(false);

                                //GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                                DestroyBeams();
                                enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                            }
                        }
                       // enemyScript.steering();

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

                        //navMeshAgent.enabled = true;

                        if (GetComponentInChildren<EnemyAttack>().return_whether_back_away())
                        { 
                            change_of_attk_type_1 += Time.deltaTime;
                        }
                        else
                        {
                            change_of_attk_type_1 = 0;
                        }

                        if (change_of_attk_type_1 >= 8.0f)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                            change_of_attk_type_1 = 0.0f;
                        }


                        //avoid the player
                        if (GetComponentInChildren<EnemyAttack>().getpostattack()
                            || change_of_attk_type_1 >= 5.0f)
                        {

                            if(GetComponentInChildren<EnemyAttack>().getpostattack())
                            {
                                change_of_attk_type_1 = 0.0f;
                            }

                            chasingspeed = 2.0f;

                            //enemyPhase =

                            //avoid player
                            //if (dist <= 5.0f)
                            //{
                            //    navMeshAgent.enabled = false;
                            //    //enemyScript.steering();
                            //    enemyScript.avoidanceCode(rand_z);
                            //}
                            ////continue chasing player
                            //else
                            //{

                            //    /*if (GetComponentInChildren<Animator>().GetBool("about2attack")
                            //        && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("aboutattack"))
                            //    {
                            //        navMeshAgent.enabled = false;
                            //    }
                            //    else
                            //    {
                            //        navMeshAgent.enabled = true;
                            //    }
                            //    GetComponent<Rigidbody>().velocity = new Vector3(rand_z, 0.0f, 0.0f);
                            //    navMeshAgent.SetDestination(playerGO.transform.position);*/
                            //}
                        }
                        //
                        //continue to chase the player
                        else
                        {
                            if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("aboutattack")
                                 || GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack"))
                            {
                                GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                                navMeshAgent.enabled = false;
                            }
                            else
                            {
                                navMeshAgent.enabled = true;
                                GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                                navMeshAgent.SetDestination(playerGO.transform.position);
                            }

                            chasingspeed = 4.0f;
                            rand_z = Random.Range(-4, 4);
                            //float rand = Random.Range(2, 8);
                            //chasingspeed = rand /*dist * 0.2f*/;
                            //GetComponent<Rigidbody>().velocity = chasingspeed * (navMeshAgent.destination - transform.position);
                            //Debug.Log("CHASING PLAYER " + GetComponent<Rigidbody>().velocity);
                            //chasingspeed = 8;
                        }
                        //

                        if (time_att_1 > 20.0f)
                        {
                            gamemanager.GetComponent<EnemyManager>().setupdating(false);
                            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                        }

                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        beam_mode = false;
                        chasingspeed = 2.0f;
                        time_att_2 = 0.0f;
                        time_att_1 = 0.0f;
                        change_of_attk_type_1 = 0.0f;

                        //enemyScript.steering();
                        //enemyScript.avoidanceCode(rand_z);
                        GetComponent<EnemyScript>().cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        beam_mode = false;
                        //chasingspeed = 0.0f;
                        chasingspeed = 2.0f;
                        time_att_2 = 0.0f;
                        time_att_1 = 0.0f;
                        change_of_attk_type_1 = 0.0f;

                        //GetComponent<Rigidbody>().velocity =

                        //if (dist <= 5.0f)
                        //{
                        //    Vector3 resultingVector = -playerGO.transform.position + transform.position;
                        //    resultingVector.y = 0;
                        //    GetComponent<Rigidbody>().velocity = resultingVector;
                        //}

                        //enemyScript.avoidanceCode(rand_z);
                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            //Debug.Log("VEL " + GetComponent<Rigidbody>().velocity);

            //Debug.Log("UPDATING FALSE");
            enemyScript.ifUpdatingfalse();
            DestroyBeams();
        }

    }

    public float return_change_of_attk_type_1()
    {
        return change_of_attk_type_1;
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
