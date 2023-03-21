using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{
    private float jumpcooldown;

    [SerializeField] NavMeshAgent navMeshAgent;

    private GameObject playerGO;

    private float speedfactor;
    private float timer;
    public EnemyScript.Phases enemyPhase;
    public float jumpHeight;
    public GameObject enemySprite;
    public float count;
    public Vector3 startpos;
    public float radius;
    Vector3 controlPoint;

    public float distance;


    public bool startupdating;


    public GameObject attackhitbox;
    // Start is called before the first frame update
    void Start()
    {

        startupdating = false;

        count = 0;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        jumpcooldown = 4.0f;
        startpos = transform.position;
        timer = 0.0f;
        /*navMeshAgent.SetDestination( 
                            //(playerGO.transform.position - transform.position) - ((playerGO.transform.position - transform.position) / 4)
                            playerGO.transform.position
                            //(playerGO.transform.position - startpos).normalized * 7.0f
            );*/
        navMeshAgent.acceleration = 20.0f;
        speedfactor = 20.0f;


        //controlPoint = startpos + (navMeshAgent.destination - transform.position) / 2 + Vector3.up * 5.0f;
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(navMeshAgent.destination, transform.position);
        enemyPhase = gameObject.GetComponent<EnemyScript>().phase;

        if (distance < GetComponent<EnemyMovement>().DetectionRange)
        {
            startupdating = true;
        }

        if (startupdating == true)
        {

            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        if (Vector3.Distance(playerGO.transform.position, transform.position) < 7.0f)
                        {
                            timer += 1 * Time.deltaTime;

                            //JUMP
                            if (timer > 1.0f)
                            {
                                //while it's jumping, disable collider;
                                if (Vector3.Distance(playerGO.transform.position, transform.position) < 0.1f)
                                {
                                    attackhitbox.GetComponent<BoxCollider>().enabled = true;
                                    GetComponent<BoxCollider>().enabled = true;
                                }
                                else
                                {
                                    attackhitbox.GetComponent<BoxCollider>().enabled = false;
                                    GetComponent<BoxCollider>().enabled = false;
                                }

                                if (Vector3.Distance(playerGO.transform.position, transform.position) < 0.5f)
                                {
                                    navMeshAgent.speed = 0;
                                }
                                else
                                {
                                    navMeshAgent.speed = 10.0f * speedfactor;
                                }
                            }
                            else
                            {
                                navMeshAgent.speed = 0.0f;
                            }
                        }
                        //continue to chase the player
                        else
                        {
                            //jumprest();

                            navMeshAgent.speed = 5.0f /** speedfactor*/;
                            startpos = transform.position;
                            timer = 0.0f;
                            navMeshAgent.SetDestination(playerGO.transform.position);
                            controlPoint = startpos + (navMeshAgent.destination - transform.position) / 2 + Vector3.up * 5.0f;
                            timer = 0.0f;
                            //startupdating = false;
                        }
                        spritejump();
                        break;
                    }
                default:
                    break;
            }
        }
        //jumprest();

        //if (timer >= 1.0f)
        //    if (Vector3.Distance(navMeshAgent.destination, transform.position) < 0.5f)
        //{

        //    count = 0;
        //    enemySprite.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);

        //    jumprest();
        //}
        ////WHILE ITS JUMPING
        //else
        //{
        //    //timer += 1 * Time.deltaTime;
        //    spritejump();

        //    //while it's jumping, disable collider;
        //    GetComponent<BoxCollider>().enabled = false;
        //    navMeshAgent.speed = 10.0f * speedfactor;
        //}
    }

    //MAKE IT CHASE THE PLAYER UNTIL IT'S NEAR THE RANGE
    public void spritejump()
    {
         if (count < 1.0f
            && timer > 1.0f)
        {
            //attackhitbox.GetComponent<BoxCollider>().enabled = false;
            //GetComponent<BoxCollider>().enabled = false;

            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startpos, controlPoint, count);
            Vector3 m2 = Vector3.Lerp(controlPoint, navMeshAgent.destination, count);
            enemySprite.transform.position = Vector3.Lerp(m1, m2, count);
        }
        else
        {
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
        attackhitbox.GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;

        enemySprite.transform.position = transform.position + new Vector3(0.0f, 0.6f, 0.0f);


        if (jumpcooldown <= 0.0f)
        {
            startpos = transform.position;
            timer = 0.0f;
            navMeshAgent.SetDestination(playerGO.transform.position
                //(playerGO.transform.position - startpos).normalized * 7.0f
                //(playerGO.transform.position - transform.position) - ((playerGO.transform.position - transform.position) / 4)
                );
            controlPoint = startpos + (navMeshAgent.destination - transform.position) / 2 + Vector3.up * 5.0f;
            navMeshAgent.acceleration = 20.0f;
            speedfactor = 20.0f;
            count = 0;

            jumpcooldown = 4.1f;

            GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
        }
        /*else
        {
            navMeshAgent.speed = 0.0f;
        }*/
    }
}
