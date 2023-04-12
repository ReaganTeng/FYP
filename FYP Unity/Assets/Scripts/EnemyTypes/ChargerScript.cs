using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChargerScript : MonoBehaviour
{
    private int number_of_bounces;
    

    private bool collided;

    private GameObject playerGO;
    Vector3 playerPos;
    Vector3 resultingVector;

    float chargingtime;



    public LayerMask lm;

    private float velocityspeed;

    public EnemyScript.Phases enemyPhase;

    [SerializeField] NavMeshAgent navMeshAgent;

    public GameObject attackhitbox;

    void Awake()
    {
        velocityspeed = 10.0f;
        number_of_bounces = 0;
        collided = false;
        navMeshAgent.enabled = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerGO.transform.position;
        resultingVector = playerPos - transform.position;
        resultingVector.y = 0;
        resultingVector.Normalize();
        chargingtime = 0.0f;
       
    }
    // Start is called before the first frame update
    void Start()
    {
        velocityspeed = 10.0f;
        number_of_bounces = 0;
        collided = false;
        navMeshAgent.enabled = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerGO.transform.position;
        resultingVector = playerPos - transform.position;
        resultingVector.y = 0;
        resultingVector.Normalize();
        chargingtime = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");

        enemyPhase = GetComponent<EnemyScript>().phase;

        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                //case EnemyScript.Phases.ATTACK_TYPE_2:

                    {

                        GetComponentInChildren<Animator>().SetBool("about2charge", true);
                        GetComponentInChildren<Animator>().SetBool("charge", true);

                        chargingtime += 1.0f * Time.deltaTime;

                        if (chargingtime < 0.1f)
                        {
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                        }

                        if (chargingtime >= 1.0f)
                        {
                            //if (chargingtime < 1.1f)
                            //{
                            //    resultingVector = playerPos - transform.position;
                            //    resultingVector.y = 0;
                            //    resultingVector.Normalize();
                            //}

                            //if (chargingtime > 1.1f
                            //    && chargingtime < 1.2f)
                            //{
                            chargingtime = 0;
                            collided = true;
                            //}
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

                        break;
                    }
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {


                        //GetComponent<MeshRenderer>().enabled = false;
                        GetComponentInChildren<Animator>().SetBool("about2charge", true);

                        GetComponentInChildren<Animator>().SetBool("charge", true);

                        chargingtime += 1.0f * Time.deltaTime;

                        if (chargingtime < 0.1f)
                        {
                            playerPos = playerGO.transform.position;
                            resultingVector = playerPos - transform.position;
                        }

                        if (chargingtime >= 1.0f)
                        {
                            if (number_of_bounces >= 2)
                            {
                                collided = true;
                                number_of_bounces = 0;
                            }
                            else
                            {
                                number_of_bounces += 1;
                                //playerPos = playerGO.transform.position;
                                //resultingVector = playerPos - transform.position;
                                chargeAtplayer();
                            }
                            chargingtime = 0.0f;
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

                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        GetComponentInChildren<Animator>().SetBool("about2charge", true);
                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
                default:
                    break;
            }
        }


        if (GetComponent<EnemyScript>().getzoneno() == 0)
        {
            resultingVector = GetComponent<EnemyScript>().getparent().position - transform.position;
            resultingVector.y = 0;
            resultingVector.Normalize();
            GetComponent<Rigidbody>().velocity = resultingVector * velocityspeed;
        }
    }


    public void chargeAtplayer()
    {
        attackhitbox.SetActive(true);

        if (enemyPhase == EnemyScript.Phases.ATTACK_TYPE_2)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        else if (enemyPhase == EnemyScript.Phases.ATTACK_TYPE_1)
        {
            GameObject bc = GameObject.FindGameObjectWithTag("playerboxcollider");
            //float distance = Vector3.Distance(transform.position, playerPos);
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
        attackhitbox.SetActive(false);
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

            GetComponentInChildren<Animator>().SetBool("charge", false);
            GetComponentInChildren<Animator>().SetBool("about2charge", false);


            //enemy.gameObject.transform.LookAt(enemy.playerGO.transform);
            GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
    }


    public void OnCollisionEnter(Collision collision)
    {
        switch (enemyPhase)
        {
            case EnemyScript.Phases.ATTACK_TYPE_1:
            //case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall"
                        || collision.gameObject.tag == "playerboxcollider"
                        || collision.gameObject.tag == "Player"
                       )
                    {
                        collided = true;

                        //if (collision.gameObject.tag == "Player"
                        //    || collision.gameObject.tag == "playerboxcollider")
                        //{
                        //    //GetComponent<BoxCollider>().enabled = false;
                        //}
                        //else
                        //{
                        //    //GetComponent<BoxCollider>().enabled = true;
                        //    collided = true;
                        //}
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    if (collision.gameObject.tag == "wall"
                        || collision.gameObject.tag == "playerboxcollider"
                        || collision.gameObject.tag == "Player"
                       )
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
                            if (collision.gameObject.tag == "Player"
                                    || collision.gameObject.tag == "playerboxcollider")
                            {
                                resultingVector = -playerPos + transform.position;
                            }
                            else
                            {
                                resultingVector = playerPos - transform.position;
                            }
                            chargeAtplayer();
                        }
                        break;
                    }
                    break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {
                    attackhitbox.SetActive(false);
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    //grab player location
                    playerPos = playerGO.transform.position;
                    //subtract between player.transform.psoition and enemy.transform.position
                    resultingVector = playerPos - transform.position;
                    chargingtime = 0.0f;
                    collided = false;

                    GetComponent<EnemyScript>().cooldownUpdate();

                    break;
                }
            /*case EnemyScript.Phases.PHASE_3:
                {
                    //unless object collided is player, don't stop
                    if (collision.gameObject.tag == "Player")
                    {
                        collided = true;
                    }
                    else
                    {
                        playerPos = playerGO.transform.position;
                        resultingVector = playerPos - transform.position;
                        chargeAtplayer();
                    }
                    break;
                }*/
            default:
                break;

        }


        
    }


   
        

        
    

}
