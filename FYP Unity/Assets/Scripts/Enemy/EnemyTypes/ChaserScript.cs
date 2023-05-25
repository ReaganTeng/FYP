using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Com.LuisPedroFonseca.ProCamera2D;


public class ChaserScript : MonoBehaviour
{
    //POST ATTACK DURATION
    float post_attack_duration;

    //TIMER OF THE OVERALL UPDATE
    float timer;

    //TIMER TO INDICATE WHEN TO AVOID PLAYER
    float timer_avoid;

    //TIMER FOR ABOUT TO ATTACK
    float delayTime;


    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE AWAY FROM THE PLAYER BY THE X AXIS
    float offset_x;
    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE AWAY FROM THE PLAYER BY THE Z AXIS
    float offset_z;


    //THE DISTANCE BEWTEEN THE PLAYER AND ENEMY
    float dist;

    //DETERMINE THE NUMBER OF HITS ENEMY CAN PERFORM
    [SerializeField] int attacks_per_session;

    int attacks_performed;

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
        post_attack_duration = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyManager>();
        navmeshagent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        enemyScript = gameObject.GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();

        attacks_performed = 0;
        enemyScript.set_enemyType(EnemyScript.EnemyType.CHASER);

        offset_x = 0;
        offset_z = 0;

        timer = 0;
        delayTime = 0;
        timer_avoid = 0;


        hitbox.GetComponent<BoxCollider>().enabled = true;
    }

    public void set_newdestination()
    {
        int rand_range_x = Random.Range(0, 11);
        int rand_range_y = Random.Range(0, 11);

        if (rand_range_x % 2 == 0)
        {
            offset_x = Random.Range(-3, 0);
        }
        else
        {
            offset_x = Random.Range(1, 4);
        }

        if (rand_range_y % 2 == 0)
        {
            offset_z = Random.Range(-3, 0);
        }
        else
        {
            offset_z = Random.Range(1, 4);
        }
    }


    // Update is called once per frame
    void Update()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();


        navmeshagent.speed = enemyScript.getnavmeshspeed();
        navmeshagent.acceleration = enemyScript.getnavmeshspeed();

        dist = Vector3.Distance(transform.position, player.transform.position);


        //ABOUT TO ATTACL
        if (anim.GetBool("about2attack")
            && anim.GetCurrentAnimatorStateInfo(0).IsName("aboutattack"))
        {
            Debug.Log("ABOUT 2 ATTACK");
            delayTime += Time.deltaTime;
            if (delayTime
                >=
                anim.GetCurrentAnimatorStateInfo(0).length)
            {
                anim.SetBool("attack", true);
            }
        }
        //


        if (hitbox.GetComponent<EnemyAttack>().get_attacking())
        {
            //ADD 1 ATTACK TO EACH LOOP
            if (hitbox.GetComponent<EnemyAttack>().get_attacking_present())
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack")
                    && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
                {
                    //DECREASE THE PLAYER'S FERVOR If PLAYER'S IN HITBOX
                    if (hitbox.GetComponent<EnemyAttack>().playerinhitbox())
                    {
                        player.GetComponent<PlayerMovement>().setHurtAnimation(true);
                        player.GetComponent<PlayerStats>().ResetConsecutiveHit();
                        //other.GetComponent<PlayerStats>().ChangeFervor(-5.0f);
                        //THICK SKIN POWERUP
                        player.GetComponent<PlayerStats>().ChangeFervor(-15.0f * player.GetComponent<PlayerStats>().getpp().return_thick_skin());
                        //
                        player.GetComponent<PlayerStats>().resetval();
                        ProCamera2DShake.Instance.ShakeUsingPreset("DamageShake");
                        //Debug.Log("HIT");
                    }
                    hitbox.GetComponent<EnemyAttack>().setattackCDtimer(
                        hitbox.GetComponent<EnemyAttack>().getattackCD());
                    attacks_performed += 1;
                }


                //END THE LOOP WHEN ATTACKS PERFORM EXCEEDED
                if (attacks_performed >= attacks_per_session)
                {
                    
                    anim.SetBool("attack", false);
                    anim.SetBool("about2attack", false);
                    hitbox.GetComponent<EnemyAttack>().setpostattack(true);
                    attacks_performed = 0;
                    delayTime = 0.0f;
                    hitbox.GetComponent<EnemyAttack>().set_attacking(false);
                    hitbox.GetComponent<EnemyAttack>().setattacking_present(false);
                    navmeshagent.enabled = true;
                }
                else
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack")
                       && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                    {
                        anim.Play("attack", 0, 0);
                    }
                }
                //
            }
            //
        }
        else
        {
            navmeshagent.enabled = true;
        }
        //

        //POST ATTACK
        if (GetComponentInChildren<EnemyAttack>().getpostattack())
        {
            post_attack_duration += Time.deltaTime;
            if (post_attack_duration >= 3.0f)
            {
                post_attack_duration = 0.0f;
                hitbox.GetComponent<EnemyAttack>().setpostattack(false);
            }
        }
        //

        if (enemyScript.getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        GetComponent<BoxCollider>().enabled = true;
                        anim.SetBool("run", true);
                        timer += Time.deltaTime;
                        
                        timer_avoid += Time.deltaTime;
                        if (timer_avoid >= 8.0f)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                            timer_avoid = 0.0f;
                        }
                      
                        //AVOID THE PLAYER WHEN ENEMY MANAGES TO HIT PLAYER, OR TIMER_AVOID >= 5
                        if (hitbox.GetComponent<EnemyAttack>().getpostattack()
                            || timer_avoid >= 5.0f)
                        {
                            enemyScript.setnavmeshspeed(4.0f);


                            if (hitbox.GetComponent<EnemyAttack>().getpostattack())
                            {
                                timer_avoid = 0.0f;
                            }

                            if (dist <= 5.0f)
                            {
                                navmeshagent.SetDestination(
                                    new Vector3(player.transform.position.x + offset_x, 
                                    transform.position.y,
                                    player.transform.position.z + offset_z)
                                    );
                            }
                        }
                        //
                        //CONTINUE TO CHASE THE PLAYER
                        else
                        {

                            enemyScript.setnavmeshspeed(2.0f);

                            set_newdestination();
                            hitbox.GetComponent<BoxCollider>().enabled = true;

                            if (anim.GetCurrentAnimatorStateInfo(0).IsName("aboutattack")
                                 || anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                            {
                                GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                                navmeshagent.enabled = false;
                            }
                            else
                            {
                                navmeshagent.enabled = true;
                                GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                                navmeshagent.SetDestination(player.transform.position);
                            }

                        }
                        //

                        //SET TO COOLDOWN MODE IF TIMER >= 20
                        if (timer > 20.0f)
                        {
                            em.setupdating(false);
                            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                        }
                        //

                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        enemyScript.setnavmeshspeed(2.0f);
                        timer = 0.0f;
                        timer_avoid = 0.0f;

                        enemyScript.cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("attack", false);
            anim.SetBool("about2attack", false);
            enemyScript.setnavmeshspeed(2.0f);
            timer = 0.0f;
            timer_avoid = 0.0f;
            enemyScript.ifUpdatingfalse();
        }

    }

    
   

}
