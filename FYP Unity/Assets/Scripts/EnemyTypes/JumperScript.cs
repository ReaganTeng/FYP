using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{

    float targetVelocity = 10.0f;
    int numberOfRays = 30;
    float angle = 90.0f;
    float rayRange = .3f;
    [SerializeField] LayerMask lm;


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

                attackhitbox = GetComponent<EnemyScript>().gethitbox();

        GetComponentInChildren<Animator>().SetBool("about2jump", false);
        GetComponentInChildren<Animator>().SetBool("jump", false);
        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
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

                        if (currentdistance < 7.0f)
                    {
                        timer += 1 * Time.deltaTime;

                        //prepare to jump
                        if (timer < 1.0f)
                        {
                            GetComponentInChildren<Animator>().SetBool("about2jump", true);

                            GetComponent<BoxCollider>().enabled = true;

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
                    //continue to chase the player
                    else
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
                        //startupdating = false;
                    }
                    //
                    break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {
                    jumpcooldown = 0.3f;
                    GetComponent<NavMeshAgent>().speed = 0.0f;
                    GetComponent<BoxCollider>().enabled = false;
                    //GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                    GetComponent<EnemyScript>().cooldownUpdate();

                    break;
                }

            case EnemyScript.Phases.ABOUT_TO_ATTACK:
                {
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
            steering();
        }
    }


    void steering()
    {
        var deltaPosition = Vector3.zero;
        for (int i = 0; i < numberOfRays; i++)
        {
            //rotate enemy angle
            var rotation = transform.rotation;
            var rotationMod = Quaternion.AngleAxis(
                 (i / ((float)numberOfRays - 1)) * angle * 2 - angle,
                 transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            var direction2 = rotation * rotationMod * Vector3.back;

            var ray = new Ray(transform.position, direction);
            var ray2 = new Ray(transform.position, direction2);

            RaycastHit hitInfo;
            //if hits something
            if (Physics.Raycast(ray, out hitInfo, rayRange
                , ~lm))
            {
                //Debug.Log("HIT SOMETHING");
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
                transform.position += deltaPosition * Time.deltaTime;

            }
            else if (Physics.Raycast(ray2, out hitInfo, rayRange
                , ~lm))
            {
                //Debug.Log("HIT SOMETHING");

                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction2;
                transform.position += deltaPosition * Time.deltaTime;
            }
        }


        /*float distance = Vector3.Distance(playerGO.transform.position, transform.position);
        if (distance < 5.0f)
        {
            navMeshAgent.speed = 0.0f;
        }
        if (distance < 5.0f)
        {
            box.size = new Vector3(box.size.x, box.size.y, 4.0f);
        }
        else
        {
            box.size = new Vector3(box.size.x, box.size.y, 1.0f);
        }*/
    }

    //MAKE IT CHASE THE PLAYER UNTIL IT'S NEAR THE RANGE
    public void spritejump(float t)
    {
         if (count < countfactor
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
            GetComponentInChildren<Animator>().SetBool("jump", false);
            GetComponentInChildren<Animator>().SetBool("about2jump", false);
            GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);

            startpos = transform.position;
            timer = 0.0f;
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
