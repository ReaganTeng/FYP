using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Com.LuisPedroFonseca.ProCamera2D;


public class ChaserScript : MonoBehaviour
{
    float timer;
    float change_of_attk_type_1;

    float delayTime;


    Transform starting_location;
    Transform ending_location;
    float dist;

    //determine how many times the player can attack at once
    [SerializeField] int attacks_per_session;

    int attacks_performed;

    GameObject player;
    EnemyManager em;
    GameObject hitbox;
    NavMeshAgent navmeshagent;
    Animator anim;
    EnemyScript.Phases enemyPhase;
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

        attacks_performed = 0;
        enemyScript.set_enemyType(EnemyScript.EnemyType.CHASER);


        timer = 0;
        delayTime = 0;
        change_of_attk_type_1 = 0;


        hitbox.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();


        navmeshagent.speed = enemyScript.getchasingspeed();
        navmeshagent.acceleration = enemyScript.getchasingspeed();

        dist = Vector3.Distance(transform.position, player.transform.position);


        //DELAY FOR CHASER
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
                //decrease the player's health
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack")
                    && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
                {
                    if (hitbox.GetComponent<EnemyAttack>().playerinhitbox())
                    {
                        player.GetComponent<PlayerMovement>().setAnimator(true);
                        player.GetComponent<PlayerStats>().ResetConsecutiveHit();
                        //other.GetComponent<PlayerStats>().ChangeFervor(-5.0f);
                        //THICK SKIN POWERUP
                        player.GetComponent<PlayerStats>().ChangeFervor(-15.0f * player.GetComponent<PlayerStats>().getpp().return_thick_skin());
                        //
                        player.GetComponent<PlayerStats>().resetval();
                        ProCamera2DShake.Instance.ShakeUsingPreset("DamageShake");
                        Debug.Log("HIT");
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



        if (enemyScript.getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        GetComponent<BoxCollider>().enabled = true;
                        anim.SetBool("chasingPlayer", true);
                        //chasingspeed = 2.0f;
                        timer += Time.deltaTime;


                        if (hitbox.GetComponent<EnemyAttack>().return_whether_back_away())
                        { 
                            change_of_attk_type_1 += Time.deltaTime;
                        }
                        else
                        {
                            change_of_attk_type_1 = 0;
                        }

                        if (change_of_attk_type_1 >= 8.0f)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                            change_of_attk_type_1 = 0.0f;
                        }

                       
                        //avoid the player
                        if (hitbox.GetComponent<EnemyAttack>().getpostattack()
                            || change_of_attk_type_1 >= 5.0f)
                        {

                            if(hitbox.GetComponent<EnemyAttack>().getpostattack())
                            {
                                change_of_attk_type_1 = 0.0f;
                            }

                            enemyScript.setchasingspeed(2.0f);

                            //enemyPhase =

                            //avoid player
                            //if (dist <= 5.0f)
                            //{
                            //    navMeshAgent.enabled = false;
                            //    //enemyScript.steering();
                            //    enemyScript.avoidanceCode(rand_z);
                            //}
                            ////continue chasing player
                            //else
                            //{

                            //    /*if (GetComponentInChildren<Animator>().GetBool("about2attack")
                            //        && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("aboutattack"))
                            //    {
                            //        navMeshAgent.enabled = false;
                            //    }
                            //    else
                            //    {
                            //        navMeshAgent.enabled = true;
                            //    }
                            //    GetComponent<Rigidbody>().velocity = new Vector3(rand_z, 0.0f, 0.0f);
                            //    navMeshAgent.SetDestination(playerGO.transform.position);*/
                            //}
                        }
                        //
                        //continue to chase the player
                        else
                        {
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

                            enemyScript.setchasingspeed(4.0f);
                        }
                        //

                        if (timer > 20.0f)
                        {
                            em.setupdating(false);
                            enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                        }

                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        enemyScript.setchasingspeed(2.0f);
                        timer = 0.0f;
                        change_of_attk_type_1 = 0.0f;

                        enemyScript.cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        

                        //GetComponent<Rigidbody>().velocity =

                        //if (dist <= 5.0f)
                        //{
                        //    Vector3 resultingVector = -playerGO.transform.position + transform.position;
                        //    resultingVector.y = 0;
                        //    GetComponent<Rigidbody>().velocity = resultingVector;
                        //}

                        //enemyScript.avoidanceCode(rand_z);
                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            //Debug.Log("VEL " + GetComponent<Rigidbody>().velocity);

            //Debug.Log("UPDATING FALSE");
            enemyScript.ifUpdatingfalse();
        }

    }

    public float return_change_of_attk_type_1()
    {
        return change_of_attk_type_1;
    }
   

}
