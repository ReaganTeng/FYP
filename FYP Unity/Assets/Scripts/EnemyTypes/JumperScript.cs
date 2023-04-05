using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{
    [SerializeField] float jumpcooldown;
    [SerializeField] EnemyScript.Phases enemyPhase;
    [SerializeField] GameObject enemySprite;
    GameObject playerGO;
    [SerializeField] GameObject spriteRenderer;


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


    public GameObject attackhitbox;
    // Start is called before the first frame update
    void Start()
    {
        startupdating = false;
        count = 0;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        startpos = transform.position;
        timer = 0.0f;
        navMeshAgent.acceleration = 20.0f;
        speedfactor = 20.0f;

        GetComponent<EnemyScript>().setabouttoattackend(3.0f); 
        GetComponent<EnemyScript>().setCoolDownEnd(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");

        enemyPhase = gameObject.GetComponent<EnemyScript>().phase;
        currentdistance = Vector3.Distance(playerGO.transform.position, transform.position);

        //Debug.Log("DISTANCE " + (int)currentdistance);

        //if (distance < GetComponent<EnemyMovement>().DetectionRange)
        //{
        //    startupdating = true;
        //}

        //if (startupdating == true)
        //{
        switch (enemyPhase)
            {


                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                {

                    if (currentdistance < 7.0f)
                    {
                        timer += 1 * Time.deltaTime;

                        //prepare to jump
                        if (timer < 1.0f)
                        {
                            GetComponentInChildren<Animator>().SetBool("about2jump", true);

                            navMeshAgent.speed = 0.0f;
                            startpos = transform.position;
                            navMeshAgent.SetDestination(playerGO.transform.position);
                            endpoint = navMeshAgent.destination;
                            controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * 15.0f;
                        }
                        //

                        //JUMP
                        if (timer > 1.5f)
                        {
                            GetComponentInChildren<Animator>().SetBool("jump", true);

                            //while it's jumping, disable collider;
                            if (currentdistance < 1.0f)
                            {
                                attackhitbox.GetComponent<BoxCollider>().enabled = true;
                                GetComponent<BoxCollider>().enabled = true;
                            }
                            else
                            {
                                attackhitbox.GetComponent<BoxCollider>().enabled = false;
                                GetComponent<BoxCollider>().enabled = false;
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
                            spritejump(0.5f);
                        }
                        else
                        {
                            navMeshAgent.speed = 0.0f;
                        }
                    }
                    //continue to chase the player
                    else
                    {
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
                        attackhitbox.GetComponent<BoxCollider>().enabled = true;

                        navMeshAgent.speed = 5.0f;
                        startpos = transform.position;
                        navMeshAgent.SetDestination(playerGO.transform.position);
                        endpoint = navMeshAgent.destination;
                        controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * 15.0f;
                        timer = 0.0f;
                        //startupdating = false;
                    }
                    //
                    break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {

                    //jumpcooldown = 4.1f;
                    jumpcooldown = 0.3f;

                    GetComponent<EnemyScript>().addtimer(1.0f * Time.deltaTime);
                    //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    GetComponent<NavMeshAgent>().speed = 0.0f;

                    attackhitbox.GetComponent<BoxCollider>().enabled = false;


                    if (GetComponent<EnemyScript>().gettimer() >=
                        GetComponent<EnemyScript>().getcooldownend())
                    {
                        GetComponent<EnemyScript>().settimer(0.0f);   
                        GetComponent<EnemyScript>().phase = EnemyScript.Phases.ABOUT_TO_ATTACK;
                    }
                    break;
                }

            case EnemyScript.Phases.ABOUT_TO_ATTACK:
                {

                    //make vibration
                    var speed = 4.0f; //how fast it shakes
                    var amount = 0.5f; //how much it shakes
                    //spriteRenderer.transform.position.x = (Mathf.Sin(Time.time * speed) * amount);
                    //spriteRenderer.transform.position = new Vector3((Mathf.Sin(Time.time * speed) * amount),
                    //spriteRenderer.transform.position.y, spriteRenderer.transform.position.z);

                    //GetComponent<MeshRenderer>().enabled = true;

                    GetComponent<EnemyScript>().abouttoattackUpdate();
                    break;
                }
            default:
                    break;
            }
        //}      
    }

    //MAKE IT CHASE THE PLAYER UNTIL IT'S NEAR THE RANGE
    public void spritejump(float t)
    {
         if (count < 1.0f
            && timer > t)
        {
            //attackhitbox.GetComponent<BoxCollider>().enabled = false;
            //GetComponent<BoxCollider>().enabled = false;

            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startpos, controlPoint, count);
            Vector3 m2 = Vector3.Lerp(controlPoint, endpoint, count);

            spriteRenderer.transform.position = Vector3.Lerp(m1, m2, count);
            enemySprite.transform.position = Vector3.Lerp(m1, m2, count);
        }
        else /*if(count > 1.0f)*/
        {
            GetComponentInChildren<Animator>().SetBool("about2jump", false);
            GetComponentInChildren<Animator>().SetBool("jump", false);
            GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);

            jumprest();
        }
    }
    

    public void jumprest()
    {
        /*switch (enemyPhase)
        {
            case EnemyScript.Phases.PHASE_1:
                {
                    jumpcooldown -= 1.0f * Time.deltaTime;

                    //navMeshAgent.speed = 0.0f;
                    break;
                }
            //let enemy constantly chase the player after jumping
            case EnemyScript.Phases.PHASE_2:
                {
                    jumpcooldown -= 1.0f * Time.deltaTime;

                    if (jumpcooldown > 0.0f)
                    {
                        Debug.Log("JUMPER CHASING PLAYER");
                        navMeshAgent.SetDestination(playerGO.transform.position);
                        navMeshAgent.acceleration = 5.0f;
                        navMeshAgent.speed = 10 * speedfactor;
                    }

                    break;
                }
            case EnemyScript.Phases.PHASE_3:
                {
                    navMeshAgent.SetDestination(playerGO.transform.position);

                    if (distance < 3.0f)
                    {
                        Debug.Log("JUMPING IN " + (int)jumpcooldown);
                        jumpcooldown -= 1.0f * Time.deltaTime;
                    }
                    else
                    {
                        jumpcooldown = 4.1f;
                    }

                    break;
                }
            default:
                break;
        }*/
        jumpcooldown -= 1.0f * Time.deltaTime;

        /*attackhitbox.GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;*/

        spriteRenderer.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);
        enemySprite.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);


        if (jumpcooldown <= 0.0f)
        {
            startpos = transform.position;
            timer = 0.0f;
            navMeshAgent.SetDestination(playerGO.transform.position
                );
            endpoint = navMeshAgent.destination;
            controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * 15.0f;
            navMeshAgent.acceleration = 20.0f;
            speedfactor = 20.0f;
            count = 0;
            GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
        }
        /*else
        {
            navMeshAgent.speed = 0.0f;
        }*/


    }
}
