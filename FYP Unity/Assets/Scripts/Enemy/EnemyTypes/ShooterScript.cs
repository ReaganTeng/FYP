using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterScript : MonoBehaviour
{
    [SerializeField] GameObject projectileGO;
    [SerializeField] int projectile_numbers;


    float shootTimer;
    float proejctilespeed;
    GameObject projectile;
    float timer;
    int projectile_shots;
    float interval_between_shots;

    EnemyScript enemyScript;
    EnemyScript.Phases enemyPhase;


    GameObject player;
    EnemyManager em;
    GameObject hitbox;
    NavMeshAgent navmeshagent;
    Animator anim;

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


        enemyScript.set_enemyType(EnemyScript.EnemyType.SHOOTER);


        proejctilespeed = 1.5f;
        shootTimer = 0;
        timer = 0;
    }


    public void shoot()
    {
        Vector3 resultingVector = player.transform.position - transform.position;
        resultingVector.y = 0;

        projectile = Instantiate(projectileGO,
        new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
        Quaternion.Euler(0, 0, 0));
        projectile.GetComponent<Rigidbody>().velocity = resultingVector * proejctilespeed;
        projectile_shots += 1;
    }


    // Update is called once per frame
    void Update()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        enemyPhase = enemyScript.return_current_phase();
        hitbox = enemyScript.returnhitbox();

        if (enemyScript.getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    if (anim.GetBool("attacked") == false)
                    {
                        timer += Time.deltaTime;
                        hitbox.GetComponent<BoxCollider>().enabled = false;
                        navmeshagent.speed = enemyScript.getchasingspeed();
                        navmeshagent.acceleration = enemyScript.getchasingspeed();

                        float dist = Vector3.Distance(transform.position, player.transform.position);
                        if (dist <= 5.0f
                            || shootTimer >= 2)
                        {
                            enemyScript.setchasingspeed(0);
                            navmeshagent.enabled = false;
                            Vector3 resultingVector = -player.transform.position + transform.position;
                            resultingVector.y = 0;
                            GetComponent<Rigidbody>().velocity = resultingVector * .5f;
                        }
                        else
                        {
                            navmeshagent.enabled = true;
                            navmeshagent.SetDestination(player.transform.position);
                            enemyScript.setchasingspeed(2);
                        }


                        anim.SetBool("chasingPlayer", true);

                        //shoot every interval
                        shootTimer += Time.deltaTime;
                        float count = 1.0f;
                        if (shootTimer >= count)
                        {
                            navmeshagent.enabled = false;
                            anim.SetBool("about2shoot", true);

                            if (shootTimer >= count + 0.7f)
                            {
                                navmeshagent.enabled = true;
                                //play attack animation
                                anim.SetBool("attack", true);
                                //
                            }
                        }
                       

                        if (anim.GetBool("attack") &&
                            anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                        {
                            enemyScript.add_transitionfromattacktimer(Time.deltaTime);

                            if (projectile_shots == 0)
                            {
                                shoot();
                            }
                           
                            if (enemyScript.return_transitionfromatktimer() >=
                            enemyScript.getCurrentAnimationLength())
                            {
                                if (projectile_shots >= projectile_numbers)
                                {
                                    shootTimer = 0.0f;

                                    projectile_shots = 0;
                                    anim.SetBool("about2shoot", false);
                                    anim.SetBool("attack", false);
                                    anim.SetBool("chasingPlayer", false);

                                    em.setupdating(false);
                                }
                                else
                                {
                                    shoot();
                                }
                                enemyScript.set_transitionfromattacktimer(0.0f);
                            }
                        }

                        if (timer >= 20.0f)
                        {
                            enemyPhase = EnemyScript.Phases.COOLDOWN;
                            em.setupdating(false);
                        }
                    }
                    else
                    {
                        shootTimer = 0.0f;
                    }
                    break;
                case EnemyScript.Phases.COOLDOWN:
                    {
                        shootTimer = 0.0f;
                        timer = 0.0f;

                        enemyScript.cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {

                        enemyScript.abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            
            enemyScript.ifUpdatingfalse();
        }
    }
}
