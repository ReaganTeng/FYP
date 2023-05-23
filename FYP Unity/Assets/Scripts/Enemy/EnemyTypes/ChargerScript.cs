using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChargerScript : MonoBehaviour
{
    //THE NUMBER OF BOUNCES THE CHARGER WILL PERFORM
    int number_of_bounces;
    //DETERMINE WHETHER ENEMY STOPS CHARGING
    bool stop_colliding;


    //THE DIRECTION THE ENEMY WILL MOVE TO USING VELOCITY
    Vector3 velocity_direction;
    //THE DISTANCE BEWTEEN PLAYER AND ENEMY
    float currentdistance;
    //HOW LONG THE PLAYER WILL CHARGE
    float chargingtime;
    //THE SPEED OF THE CHARGING
    float velocityspeed;
    //THE PLAYER'S LAST KNOWN POSITION BEFOR ECHARGING
    Vector3 playerPos;

    //THE PIVOTPOINT GAMEOBJECT
    [SerializeField] GameObject pivotpointGO;
    GameObject pivotpoint;
    //THE LOCKONBEAM GAMEOBJECT
    [SerializeField]GameObject lockonbeamGO;
    GameObject lockonbeam;


    //THE PLAYER PREFAB
    GameObject player;
    //THE ENEMY MANAGER SCRIPT IN GAME MANAGER PREFAB
    EnemyManager em;
    //ENEMY'S HITBOX
    GameObject hitbox;
    //ENEMY'S NAVMESHAGENT
    NavMeshAgent navmeshagent;
    //ENEMY'S ANIMATION CONTROLLER
    Animator anim;
    //ENEMEY'S PHASE IN ENEMYSCRIPT
    EnemyScript.Phases enemyPhase;
    //ENEMY'S ENEMYSCRIPT
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
        stop_colliding = false;
        
        velocity_direction = player.transform.position - transform.position;
        velocity_direction.y = 0;
        velocity_direction.Normalize();
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
                            velocity_direction = playerPos - transform.position;
                        }
                        if (chargingtime >= 1.0f)
                        {
                            chargingtime = 0;
                            stop_colliding = true;
                        }
                        //KEEP GOING FORWARD UNTIL HITS WALL
                        if (stop_colliding)
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
                            velocity_direction = playerPos - transform.position;
                        }

                        if (chargingtime >= 2.0f)
                        {
                            playerPos = player.transform.position;
                            velocity_direction = playerPos - transform.position;
                            number_of_bounces += 1;
                            chargeAtplayer();
                            chargingtime = 0.0f;
                        }


                        if (number_of_bounces >= 3)
                        {
                            stop_colliding = true;
                            number_of_bounces = 0;
                        }

                        //KEEP GOING FORWARD UNTIL HITS WALL
                        if (stop_colliding)
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
                        stop_colliding = false;
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
                        

                        if (currentdistance <= 4.0f)
                        {
                            //BACK AWAY
                            velocity_direction = -playerPos + transform.position;
                            GetComponent<Rigidbody>().velocity = velocity_direction;
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
            velocity_direction = GetComponent<EnemyScript>().getparent().position - transform.position;
            velocity_direction.y = 0;
            velocity_direction.Normalize();
            GetComponent<Rigidbody>().velocity = velocity_direction * velocityspeed;
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
        velocity_direction.y = 0;
        //normalise resulting vector
        velocity_direction.Normalize();

        //CHARGE TOWARDS THE PLAYER'S OVERALL DIRECTION, velocity = resulting vector
        GetComponent<Rigidbody>().velocity = velocity_direction  * velocityspeed; 
    }


    public void recovering()
    {
        hitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
       
        //grab player location
        playerPos = player.transform.position;
        //subtract between player.transform.psoition and enemy.transform.position
        velocity_direction = playerPos - transform.position;
        chargingtime = 0.0f;
        stop_colliding = false;

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
                        stop_colliding = true;
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    
                    if  (collision.gameObject.tag == "Player")
                    {
                        playerPos = player.transform.position;
                        velocity_direction = -playerPos + transform.position;
                        stop_colliding = true;
                        number_of_bounces = 0;
                    }
                    else
                    {
                       
                        number_of_bounces += 1;
                        playerPos = player.transform.position;
                        velocity_direction = playerPos - transform.position;

                        chargeAtplayer();
                        
                        break;
                    }
                    break;
                }


            default:
                break;

        }


    
    }

    void OnTriggerEnter(Collider collision)
    {
        switch (enemyPhase)
        {
            case EnemyScript.Phases.ATTACK_TYPE_1:
                {
                    if (collision.gameObject.tag == "wall"
                       )
                    {
                        stop_colliding = true;
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall")
                    {
                        number_of_bounces += 1;
                        playerPos = player.transform.position;
                        velocity_direction = playerPos - transform.position;
                        chargeAtplayer();   
                    }
                    break;
                }


            default:
                break;

        }
    }


}
