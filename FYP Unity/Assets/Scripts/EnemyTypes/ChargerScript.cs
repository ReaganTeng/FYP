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
    float currentdistance;

    float chargingtime;

    [SerializeField] LayerMask lm;

    private float velocityspeed;

    EnemyScript.Phases enemyPhase;

    [SerializeField] NavMeshAgent navMeshAgent;

    public GameObject attackhitbox;

    EnemyScript enemyScript;


    //[SerializeField] GameObject weaponobject;
    //[SerializeField] GameObject weaponobjec2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyScript>().set_enemyType(EnemyScript.EnemyType.CHARGER);

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
        currentdistance = Vector3.Distance(playerGO.transform.position, transform.position);

        //weaponobject.SetActive(true);
        //weaponobjec2.SetActive(false);

        //if (weaponobject == null)
        //{
        //    weaponobject = Instantiate(gameObject,
        //            weaponobject.transform.position, weaponobject.transform.rotation);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = GetComponent<EnemyScript>().return_current_phase();


        //if(Input.GetKey(KeyCode.T) )
        //{

        //    weaponobject.SetActive(false);
        //    weaponobjec2.SetActive(true);
        //}
        //else
        //{
        //    weaponobject.SetActive(true);
        //    weaponobjec2.SetActive(false);

        //}




        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                    //case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        GetComponentInChildren<Animator>().SetBool("charge", true);
                        chargingtime += 1.0f * Time.deltaTime;

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

                        break;
                    }
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
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
                case EnemyScript.Phases.COOLDOWN:
                    {
                        attackhitbox.GetComponent<BoxCollider>().enabled = false;
                        GetComponent<BoxCollider>().enabled = true;
                        //grab player location
                        playerPos = playerGO.transform.position;
                        //subtract between player.transform.psoition and enemy.transform.position

                        chargingtime = 0.0f;
                        collided = false;

                        GetComponentInChildren<Animator>().SetBool("charge", false);
                        GetComponentInChildren<Animator>().SetBool("about2charge", false);

                        GetComponent<EnemyScript>().cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

                        //if (currentdistance <= 5.0f)
                        //{
                            //BACK AWAY
                            resultingVector = -playerPos + transform.position;
                            GetComponent<Rigidbody>().velocity = resultingVector;
                            //
                        //}
                        //Debug.Log("CHARGER VELOCITY " + GetComponent<Rigidbody>().velocity);

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

        

        enemyScript.steering();

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
                    if (collision.gameObject.tag == "wall"
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
                            if (collision.gameObject.tag == "Player")
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
            
           
            default:
                break;

        }


        
    }


   
        

        
    

}
