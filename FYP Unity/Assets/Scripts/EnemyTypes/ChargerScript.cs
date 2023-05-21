using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChargerScript : MonoBehaviour
{
    int number_of_bounces;
    bool collided;
    Vector3 resultingVector;
    float currentdistance;
    float chargingtime;
    [SerializeField] LayerMask lm;
    float velocityspeed;
    Vector3 playerPos;

    GameObject pivotpoint;
    [SerializeField] GameObject pivotpointGO;
    [SerializeField]GameObject lockonbeamGO;
     GameObject lockonbeam;


    GameObject player;
    EnemyManager em;
    GameObject hitbox;
    NavMeshAgent navmeshagent;
    Animator anim;
    EnemyScript.Phases enemyPhase;
    EnemyScript enemyScript;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyManager>();
        navmeshagent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        enemyScript = gameObject.GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();

        enemyScript.set_enemyType(EnemyScript.EnemyType.CHARGER);
        velocityspeed =8.5f;
        number_of_bounces = 0;
        collided = false;
        
        resultingVector = player.transform.position - transform.position;
        resultingVector.y = 0;
        resultingVector.Normalize();
        chargingtime = 0.0f;
        currentdistance = Vector3.Distance(player.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();

        if (enemyScript.getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                    {
                        DestroyBeams();
                        navmeshagent.enabled = false;
                        anim.SetBool("charge", true);
                        chargingtime += 1.0f * Time.deltaTime;
                        if (chargingtime < 0.1f)
                        {
                            playerPos = player.transform.position;
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

                        anim.SetBool("charge", true);

                        chargingtime += 1.0f * Time.deltaTime;

                        navmeshagent.enabled = false;


                        if (chargingtime < 0.1f)
                        {
                            playerPos = player.transform.position;
                            resultingVector = playerPos - transform.position;
                        }

                        if (chargingtime >= 2.0f)
                        {
                            playerPos = player.transform.position;
                            resultingVector = playerPos - transform.position;
                            number_of_bounces += 1;
                            chargeAtplayer();
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
                        navmeshagent.enabled = true;

                        hitbox.GetComponent<BoxCollider>().enabled = false;
                        GetComponent<BoxCollider>().enabled = true;
                        //grab player location
                        playerPos = player.transform.position;
                        //subtract between player.transform.psoition and enemy.transform.position

                        chargingtime = 0.0f;
                        collided = false;
                        anim.SetBool("chasingPlayer", false);
                        anim.SetBool("charge", false);
                        anim.SetBool("about2charge", false);

                        //enemyScript.steering();
                        //enemyScript.steering_3();
                        //enemyScript.avoidanceCode(rand_z);


                        enemyScript.cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        navmeshagent.enabled = true;

                        
                        //Debug.Log("AAA");
                        Transform ending_location = player.transform;
                        if (lockonbeam == null)
                        {
                            //Debug.Log("INSTANTIATE BEAM");
                            pivotpoint = Instantiate(pivotpointGO,
                                transform.position,
                                Quaternion.Euler(0, 0, 0));
                            pivotpoint.transform.SetParent(
                                GetComponentInChildren<SpriteRenderer>().transform);
                            lockonbeam = Instantiate(lockonbeamGO,
                                transform.position,
                                Quaternion.Euler(0, 0, 0));
                            lockonbeam.transform.localScale =
                                new Vector3(.05f,
                                lockonbeamGO.transform.localScale.y,
                                10 / 10);
                            lockonbeam.transform.SetParent(pivotpoint.transform);

                        }
                        //
                        pivotpoint.transform.LookAt(
                                new Vector3(ending_location.position.x, transform.position.y, ending_location.position.z));
                        

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
                        anim.SetBool("about2charge", true);
                       enemyScript.abouttoattackUpdate();
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

        if (enemyScript.getzoneno() == 0)
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
        if (pivotpoint != null)
        {
            Destroy(pivotpoint);
        }
    }

    public void chargeAtplayer()
    {

        DestroyBeams();
        hitbox.GetComponent<BoxCollider>().enabled = true;

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
        hitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
       
        //grab player location
        playerPos = player.transform.position;
        //subtract between player.transform.psoition and enemy.transform.position
        resultingVector = playerPos - transform.position;
        chargingtime = 0.0f;
        collided = false;

        enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
        em.GetComponent<EnemyManager>().setupdating(false);
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
                        playerPos = player.transform.position;
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
                            playerPos = player.transform.position;
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
                            playerPos = player.transform.position;
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
