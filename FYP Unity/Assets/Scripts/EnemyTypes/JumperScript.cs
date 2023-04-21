using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{

   

    [SerializeField] float jumpcooldown;
    EnemyScript.Phases enemyPhase;
    [SerializeField] GameObject enemySprite;
    GameObject playerGO;
    [SerializeField] GameObject spriteRenderer;
    [SerializeField] float jumpheight;

    [SerializeField] float jumpspeed;
    [SerializeField] float countfactor;

    float speedfactor;
    

    //FOR SPRITE JUMPING
    float count;
    Vector3 startpos;
    Vector3 controlPoint;
    Vector3 endpoint;
    //


    float timer;


    float currentdistance;
    public NavMeshAgent navMeshAgent;

    public bool startupdating;
    EnemyScript enemyScript;


    /*[SerializeField] */
    GameObject attackhitbox;
    bool jumpmode;

   
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyScript>().set_enemyType(EnemyScript.EnemyType.JUMPER);
        startupdating = false;
        count = 0;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        startpos = transform.position;
        timer = 0.0f;
        navMeshAgent.acceleration = 20.0f;
        speedfactor = 20.0f;
        jumpmode = false;
        attackhitbox = GetComponent<EnemyScript>().gethitbox();
    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        enemyScript = GetComponent<EnemyScript>();

        enemyPhase = GetComponent<EnemyScript>().return_current_phase();
        currentdistance = Vector3.Distance(playerGO.transform.position, transform.position);

        //Debug.Log("DISTANCE " + (int)currentdistance);

        //if (distance < GetComponent<EnemyMovement>().DetectionRange)
        //{
        //    startupdating = true;
        //}

        attackhitbox = GetComponent<EnemyScript>().gethitbox();

        //if (startupdating == true)
        //{

        if (GetComponent<EnemyScript>().getupdating())
        { 
        switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                        //attackhitbox.GetComponent<BoxCollider>().enabled = true;

                    if (currentdistance < 4.5f)
                    {
                            jumpmode = true;
                    }
                    //continue to chase the player
                    if(jumpmode == false /*&&
                            currentdistance >= 4.5f*/)
                    {
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
                        GetComponent<BoxCollider>().enabled = true;
                        attackhitbox.GetComponent<BoxCollider>().enabled = true;

                        navMeshAgent.speed = 5.0f;
                        startpos = transform.position;
                        navMeshAgent.SetDestination(playerGO.transform.position);
                        endpoint = navMeshAgent.destination;
                        controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
                        timer = 0.0f;
                    }
                    //

                        
                        if (jumpmode)
                        {
                            timer += Time.deltaTime;

                            //prepare to jump
                            if (timer < 1.0f)
                            {
                                GetComponentInChildren<Animator>().SetBool("about2jump", true);
                                GetComponent<BoxCollider>().enabled = true;

                                //BACK AWAY
                                Vector3 resultingVector = -playerGO.transform.position + transform.position;
                                GetComponent<Rigidbody>().velocity = resultingVector * 0.2f;
                                //

                                navMeshAgent.speed = 0.0f;
                                startpos = transform.position;
                                navMeshAgent.SetDestination(playerGO.transform.position);
                                endpoint = navMeshAgent.destination;
                                controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
                            }
                            //

                            //JUMP
                            if (timer > 1.5f)
                            {
                                GetComponentInChildren<Animator>().SetBool("jump", true);

                                GetComponent<BoxCollider>().enabled = false;

                                //while it's jumping, disable attackhitbox;
                                if (currentdistance < 1.0f)
                                {
                                    attackhitbox.GetComponent<BoxCollider>().enabled = true;
                                }
                                else
                                {
                                    attackhitbox.GetComponent<BoxCollider>().enabled = false;
                                }
                                //

                                if (currentdistance < 0.5f)
                                {
                                    navMeshAgent.speed = 0;
                                }
                                else
                                {
                                    navMeshAgent.speed = 10.0f * speedfactor;
                                }

                                spritejump(jumpspeed);
                            }
                            else
                            {
                                navMeshAgent.speed = 0.0f;
                            }

                        }
                        break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {
                    jumpmode = false;
                    jumpcooldown = 0.3f;
                    GetComponent<NavMeshAgent>().speed = 0.0f;
                    GetComponent<EnemyScript>().cooldownUpdate();

                    break;
                }

            case EnemyScript.Phases.ABOUT_TO_ATTACK:
                {
                        if (currentdistance <= 5.0f)
                        {
                            //BACK AWAY
                            Vector3 resultingVector = -playerGO.transform.position + transform.position;
                            GetComponent<Rigidbody>().velocity = resultingVector;
                            //
                        }

                    //GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                    GetComponent<NavMeshAgent>().speed = 0.0f;
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

    }




    //MAKE IT CHASE THE PLAYER UNTIL IT'S NEAR THE RANGE
    public void spritejump(float t)
    {
         if (count < countfactor
            && timer > t)
        {
            

            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startpos, controlPoint, count);
            Vector3 m2 = Vector3.Lerp(controlPoint, endpoint, count);

            spriteRenderer.transform.position = Vector3.Lerp(m1, m2, count);
            enemySprite.transform.position = Vector3.Lerp(m1, m2, count);
        }
         //if player landed on ground
        else /*if(count > 1.0f)*/
        {

            jumprest();
        }
    }
    

    public void DestroyBeams()
    {

    }

    public void jumprest()
    {
        jumpcooldown -= 1.0f * Time.deltaTime;

        spriteRenderer.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);
        enemySprite.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

        if (jumpcooldown <= 0.0f)
        {
            startpos = transform.position;
            timer = 0.0f;

            GetComponentInChildren<Animator>().SetBool("about2jump", false);
            GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
            GetComponentInChildren<Animator>().SetBool("jump", false);

            navMeshAgent.SetDestination(playerGO.transform.position
                );
            endpoint = navMeshAgent.destination;
            controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
            navMeshAgent.acceleration = 20.0f;
            speedfactor = 20.0f;
            count = 0;
            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
        }
       
    }
}
