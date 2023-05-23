using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperScript : MonoBehaviour
{
    //THE COOLDOWN PERIOD AFTER JUMPING
    [SerializeField] float jumpcooldown;
    float jumpcooldown_timer;


    ///FOR SPRITE JUMPING
    //THE CANVAS OF THE ENEMY
    [SerializeField] GameObject jumperCanvas;
    //THE SPRITE OF THE ENEMY
    [SerializeField] GameObject spriteRenderer;
    //CONTROLS THE HEIGHT OF THE SPRITE WHEN JUMPING
    [SerializeField] float jumpheight;
    //CONTROLS THE SPEED OF THE JUMP
    [SerializeField] float jumpspeed;
    [SerializeField] AnimationClip jumpClip;
    float count;
    Vector3 startpos;
    Vector3 controlPoint;
    Vector3 endpoint;
    ///


    float speedfactor;



    //THE DISTANCE BETWEEN PLAYER AND ENEMY
    float currentdistance;
    float timer;
    bool jumpmode;


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
        navmeshagent.acceleration = 20.0f;

        enemyScript.set_enemyType(EnemyScript.EnemyType.JUMPER);
        count = 0;
        startpos = transform.position;
        timer = 0.0f;
        speedfactor = 20.0f;
        jumpmode = false;
    }

    // Update is called once per frame
    void Update()
    {
        navmeshagent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        enemyScript = gameObject.GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();


        currentdistance = Vector3.Distance(player.transform.position, transform.position);

        

        if (enemyScript.getupdating())
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
                        anim.SetBool("run", true);
                        GetComponent<BoxCollider>().enabled = true;
                        hitbox.GetComponent<BoxCollider>().enabled = true;
                        navmeshagent.speed = 5.0f;
                        startpos = transform.position;
                        navmeshagent.SetDestination(player.transform.position);
                        endpoint = navmeshagent.destination;
                        controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
                        timer = 0.0f;
                    }
                    //

                        
                    if (jumpmode)
                    {
                        timer += Time.deltaTime;

                        //PREPARE TO JUMP
                        if (timer < enemyScript.getCurrentAnimationLength())
                        {
                            anim.SetBool("about2jump", true);
                            GetComponent<BoxCollider>().enabled = true;

                            //BACK AWAY
                            Vector3 resultingVector = -player.transform.position + transform.position;
                            GetComponent<Rigidbody>().velocity = resultingVector * 0.2f;
                            //

                            navmeshagent.speed = 0.0f;
                            startpos = transform.position;
                                navmeshagent.SetDestination(player.transform.position);
                            endpoint = navmeshagent.destination;
                            controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
                        }
                        //

                        //JUMP
                        if (timer > enemyScript.getCurrentAnimationLength() + 0.1f)
                        {
                             anim.SetBool("jump", true);
                            GetComponent<BoxCollider>().enabled = false;

                            if (currentdistance < 0.5f)
                            {
                                navmeshagent.speed = 0;
                            }
                            else
                            {
                                float jumpdistance
                                            = Vector3.Distance(startpos, endpoint);
                                if (currentdistance >= jumpdistance/2)
                                {
                                        navmeshagent.speed = 10.0f * speedfactor * jumpspeed;
                                }
                                else
                                {
                                        navmeshagent.speed = 10.0f * speedfactor * jumpspeed * 5;
                                }
                            }

                            spritejump();
                        }
                        else
                        {
                                navmeshagent.speed = 0.0f;
                        }

                    }
                    break;
                }
            case EnemyScript.Phases.COOLDOWN:
                {
                    jumpmode = false;
                    jumpcooldown_timer = jumpcooldown;
                    enemyScript.setnavmeshspeed(0.0f);
                    enemyScript.cooldownUpdate();

                    break;
                }

            case EnemyScript.Phases.ABOUT_TO_ATTACK:
                {

                    enemyScript.setnavmeshspeed(0.0f);
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

            if (count < .85f)
            {
                hitbox.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                hitbox.GetComponent<BoxCollider>().enabled = true;
            }


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
        else 
        {
            jumprest();
        }
    }
    //

    

    public void jumprest()
    {
        jumpcooldown_timer -= 1.0f * Time.deltaTime;

        spriteRenderer.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
        jumperCanvas.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
        hitbox.GetComponent<BoxCollider>().enabled = false;

        if (jumpcooldown_timer <= 0.0f)
        {
            startpos = transform.position;
            timer = 0.0f;

            anim.SetBool("about2jump", false);
            anim.SetBool("run", false);
            anim.SetBool("jump", false);

            navmeshagent.SetDestination(player.transform.position
                );
            endpoint = navmeshagent.destination;
            controlPoint = startpos + (endpoint - transform.position) / 2 + Vector3.up * jumpheight;
            navmeshagent.acceleration = 20.0f;
            speedfactor = 20.0f;
            count = 0;
            em.GetComponent<EnemyManager>().setupdating(false);

            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
        }
       
    }
}
