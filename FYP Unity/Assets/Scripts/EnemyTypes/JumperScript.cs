using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{
    [SerializeField] float jumpcooldown;
    EnemyScript.Phases enemyPhase;
    [SerializeField] GameObject jumperCanvas;
    GameObject playerGO;
    [SerializeField] GameObject spriteRenderer;
    [SerializeField] float jumpheight;

    [SerializeField] float jumpspeed;

    float speedfactor;

    [SerializeField] AnimationClip jumpClip;

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

    int rand_z;

    GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        rand_z = 0;
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
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        playerGO = GameObject.FindGameObjectWithTag("Player");
        enemyScript = GetComponent<EnemyScript>();

        enemyPhase = GetComponent<EnemyScript>().return_current_phase();
        currentdistance = Vector3.Distance(playerGO.transform.position, transform.position);

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
                    if (currentdistance < 6.5f)
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
                        if (timer < enemyScript.getCurrentAnimationLength())
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
                        if (timer > enemyScript.getCurrentAnimationLength() + 0.1f)
                        {
                            GetComponentInChildren<Animator>().SetBool("jump", true);
                            GetComponent<BoxCollider>().enabled = false;

                            //while it's jumping, disable attackhitbox;
                            if (currentdistance < 1.5f)
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
                                float jumpdistance
                                            = Vector3.Distance(startpos, endpoint);
                                if (currentdistance >= jumpdistance/2)
                                {
                                    navMeshAgent.speed = 10.0f * speedfactor * jumpspeed;
                                }
                                else
                                {
                                    navMeshAgent.speed = 10.0f * speedfactor * jumpspeed * 5;
                                }
                            }

                            spritejump();
                        }
                        else
                        {
                            navMeshAgent.speed = 0.0f;
                        }

                    }

                        //enemyScript.steering();


                        break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {
                    jumpmode = false;
                    jumpcooldown = 0.3f;

                    //enemyScript.steering();
                    //enemyScript.steering_3();
                    enemyScript.avoidanceCode(rand_z);

                    GetComponent<NavMeshAgent>().speed = 0.0f;
                    GetComponent<EnemyScript>().cooldownUpdate();

                    break;
                }

            case EnemyScript.Phases.ABOUT_TO_ATTACK:
                {
                        //if (currentdistance <= 5.0f)
                        //{
                        //    //BACK AWAY
                        //    Vector3 resultingVector = -playerGO.transform.position + transform.position;
                        //    GetComponent<Rigidbody>().velocity = resultingVector;
                        //    //
                        //}

                        //enemyScript.steering();
                        //enemyScript.steering_3();
                        //enemyScript.avoidanceCode(rand_z);

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

        //enemyScript.steering();

    }




    //TRANSLATE THE SPRITE IN AN ARC
    public void spritejump()
    {
        GetComponent<BoxCollider>().enabled = false;

        if (count < 1.0f
           && timer > jumpClip.length)
        {
            count += jumpspeed * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startpos, controlPoint, count);
            Vector3 m2 = Vector3.Lerp(controlPoint, endpoint, count);

            spriteRenderer.transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, m1.y, transform.position.z),
                new Vector3(transform.position.x, m2.y, transform.position.z), 
                count);
            jumperCanvas.transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, m1.y, transform.position.z),
                new Vector3(transform.position.x, m2.y, transform.position.z),
                count);
        }
         //if player landed on ground
        else /*if(count > 1.0f)*/
        {
            jumprest();
        }
    }
    //

    

    public void jumprest()
    {
        jumpcooldown -= 1.0f * Time.deltaTime;

        spriteRenderer.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
        jumperCanvas.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
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
            gamemanager.GetComponent<EnemyManager>().setupdating(false);

            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
        }
       
    }
}
