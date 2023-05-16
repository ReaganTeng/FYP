using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChargerScript : MonoBehaviour
{
    int number_of_bounces;
    bool collided;
    GameObject playerGO;
    Vector3 playerPos;
    Vector3 resultingVector;
    float currentdistance;
    float chargingtime;
    [SerializeField] LayerMask lm;
    float velocityspeed;
    EnemyScript.Phases enemyPhase;
    [SerializeField] NavMeshAgent navMeshAgent;
    public GameObject attackhitbox;
    EnemyScript enemyScript;

    int rand_z;

    GameObject gamemanager;

    GameObject pivot;
    [SerializeField] GameObject pivotpoint;

    [SerializeField]GameObject lockon;
     GameObject lockonbeam;


    // Start is called before the first frame update
    void Start()
    {
        rand_z = 0;

        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        GetComponent<EnemyScript>().set_enemyType(EnemyScript.EnemyType.CHARGER);
        velocityspeed =8.5f;
        number_of_bounces = 0;
        collided = false;
        navMeshAgent.enabled = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerGO.transform.position;
        resultingVector = playerPos - transform.position;
        resultingVector.y = 0;
        resultingVector.Normalize();
        chargingtime = 0.0f;
        currentdistance = Vector3.Distance(playerGO.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = GetComponent<EnemyScript>().return_current_phase();

        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                    //case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        DestroyBeams();

                        navMeshAgent.enabled = false;

                        GetComponentInChildren<Animator>().SetBool("charge", true);
                        chargingtime += 1.0f * Time.deltaTime;

                        rand_z = Random.Range(-4, 4);

                        if (chargingtime < 0.1f)
                        {
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                        }

                        if (chargingtime >= 1.0f)
                        {

                            chargingtime = 0;
                            collided = true;

                        }
                        //KEEP GOING FORWARD UNTIL HITS WALL
                        if (collided)
                        {
                            recovering();
                        }
                        else
                        {
                            chargeAtplayer();
                        }
                        //enemyScript.steering();

                        break;
                    }
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        DestroyBeams();

                        GetComponentInChildren<Animator>().SetBool("charge", true);

                        chargingtime += 1.0f * Time.deltaTime;

                        navMeshAgent.enabled = false;

                        rand_z = Random.Range(-4, 4);

                        if (chargingtime < 0.1f)
                        {
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                        }

                        if (chargingtime >= 2.0f)
                        {
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                            //if (number_of_bounces >= 2)
                            //{
                            //    collided = true;
                            //    number_of_bounces = 0;
                            //}
                            //else
                            //{
                            //Debug.Log("BOUNCE 2");
                                number_of_bounces += 1;
                                chargeAtplayer();
                            //}
                            chargingtime = 0.0f;
                        }


                        if (number_of_bounces >= 3)
                        {
                            collided = true;
                            number_of_bounces = 0;
                        }

                        //KEEP GOING FORWARD UNTIL HITS WALL
                        if (collided)
                        {
                            recovering();
                        }
                        else
                        {
                            chargeAtplayer();
                        }
                        //enemyScript.steering();

                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        DestroyBeams();
                        navMeshAgent.enabled = true;

                        attackhitbox.GetComponent<BoxCollider>().enabled = false;
                        GetComponent<BoxCollider>().enabled = true;
                        //grab player location
                        playerPos = playerGO.transform.position;
                        //subtract between player.transform.psoition and enemy.transform.position

                        chargingtime = 0.0f;
                        collided = false;
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);


                        GetComponentInChildren<Animator>().SetBool("charge", false);
                        GetComponentInChildren<Animator>().SetBool("about2charge", false);

                        //enemyScript.steering();
                        //enemyScript.steering_3();
                        //enemyScript.avoidanceCode(rand_z);


                        GetComponent<EnemyScript>().cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        navMeshAgent.enabled = true;

                        if (enemyScript.return_attackptn() != EnemyScript.AttackPattern.PATTERN_3)
                        {
                            //Debug.Log("AAA");
                            Transform ending_location = playerGO.transform;
                            if (lockonbeam == null)
                            {
                                //Debug.Log("INSTANTIATE BEAM");
                                pivot = Instantiate(pivotpoint,
                                   transform.position,
                                   Quaternion.Euler(0, 0, 0));
                                pivot.transform.SetParent(
                                    GetComponentInChildren<SpriteRenderer>().transform);
                                lockonbeam = Instantiate(lockon,
                                    transform.position,
                                    Quaternion.Euler(0, 0, 0));
                                lockonbeam.transform.localScale =
                                    new Vector3(.05f,
                                    lockon.transform.localScale.y,
                                    10 / 10);
                                lockonbeam.transform.SetParent(pivot.transform);

                            }
                            //
                            pivot.transform.LookAt(
                                    new Vector3(ending_location.position.x, transform.position.y, ending_location.position.z));
                        }

                        if (currentdistance <= 5.0f)
                        {
                            //BACK AWAY
                            resultingVector = -playerPos + transform.position;
                            GetComponent<Rigidbody>().velocity = resultingVector;
                            //
                        }
                        //enemyScript.steering();
                        //enemyScript.steering_3();
                        //enemyScript.avoidanceCode(rand_z);

                        //GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                        //GetComponentInChildren<Animator>().SetBool("charge", false);
                        GetComponentInChildren<Animator>().SetBool("about2charge", true);
                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
                default:
                    break;
            }
        }
        else
        {
            enemyScript.ifUpdatingfalse();
        }

        //enemyScript.steering();
        if (GetComponent<EnemyScript>().getzoneno() == 0)
        {
            resultingVector = GetComponent<EnemyScript>().getparent().position - transform.position;
            resultingVector.y = 0;
            resultingVector.Normalize();
            GetComponent<Rigidbody>().velocity = resultingVector * velocityspeed;
        }
    }



    public void DestroyBeams()
    {
       
        if (lockonbeam != null)
        {
            Destroy(lockonbeam);
        }
        if (pivot != null)
        {
            Destroy(pivot);
        }
    }

    public void chargeAtplayer()
    {

        DestroyBeams();

        //attackhitbox.SetActive(true);
        attackhitbox.GetComponent<BoxCollider>().enabled = true;

        if (enemyPhase == EnemyScript.Phases.ATTACK_TYPE_2)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        else if (enemyPhase == EnemyScript.Phases.ATTACK_TYPE_1)
        {
            GameObject bc = GameObject.FindGameObjectWithTag("Player");
            float distance = Vector3.Distance(transform.position, bc.transform.position);
            //Debug.Log("DISTANCE " + (int)distance);
            if ((int)distance <= 1)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            else if ((int)distance > 1)
            {
                GetComponent<BoxCollider>().enabled = true;
            }
        }

        //resulting vector.y = 0
        resultingVector.y = 0;
        //normalise resulting vector
        resultingVector.Normalize();

        //CHARGE TOWARDS THE PLAYER'S OVERALL DIRECTION, velocity = resulting vector
        GetComponent<Rigidbody>().velocity = resultingVector  * velocityspeed; 
    }


    public void recovering()
    {
        //attackhitbox.SetActive(false);
        attackhitbox.GetComponent<BoxCollider>().enabled = false;


        GetComponent<BoxCollider>().enabled = true;
        //Debug.Log("Recovering");

        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //chargecooldown -= 1.0f * Time.deltaTime;
       
        //grab player location
        playerPos = playerGO.transform.position;
        //subtract between player.transform.psoition and enemy.transform.position
        resultingVector = playerPos - transform.position;
        chargingtime = 0.0f;
        collided = false;



        enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
        gamemanager.GetComponent<EnemyManager>().setupdating(false);
    }


    public void OnCollisionEnter(Collision collision)
    {
        switch (enemyPhase)
        {
            case EnemyScript.Phases.ATTACK_TYPE_1:
                //case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall"
                        || collision.gameObject.tag == "Player"
                       )
                    {
                        collided = true;
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    
                    if  (collision.gameObject.tag == "Player")
                    {
                        playerPos = playerGO.transform.position;
                        resultingVector = -playerPos + transform.position;
                        collided = true;
                        number_of_bounces = 0;
                    }
                    else
                    {
                        //if (number_of_bounces >= 2)
                        //{
                        //    collided = true;
                        //    number_of_bounces = 0;
                        //}
                        //else
                        {
                            number_of_bounces += 1;
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;


                            //playerPos = playerGO.transform.position;
                            //if (collision.gameObject.tag == "Player")
                            //{
                            //    resultingVector = -playerPos + transform.position;
                            //}
                            //else
                            //{
                            //    resultingVector = playerPos - transform.position;
                            //}

                            chargeAtplayer();
                        }
                        break;
                    }
                    break;
                }


            default:
                break;

        }


        /*if (collision.gameObject.tag == "wall")
        {
            if (number_of_bounces >= 3)
            {
                collided = true;
                number_of_bounces = 0;
            }
            else
            {
                number_of_bounces += 1;
                playerPos = playerGO.transform.position;
                
                resultingVector = playerPos - transform.position;
                
                chargeAtplayer();
            }
        }
         else if(collision.gameObject.tag == "Player")
        {
            collided = true;
        }*/
    }

    void OnTriggerEnter(Collider collision)
    {
        switch (enemyPhase)
        {
            case EnemyScript.Phases.ATTACK_TYPE_1:
                //case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall"
                       )
                    {
                        collided = true;
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall")
                    {
                        //if (number_of_bounces >= 2)
                        //{
                        //    collided = true;
                        //    number_of_bounces = 0;
                        //}
                        //else
                        {
                            number_of_bounces += 1;
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                            chargeAtplayer();
                        }
                        break;
                    }
                    break;
                }


            default:
                break;

        }
    }


}
