using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RangerScript : MonoBehaviour
{
    //THE LOCKONBEAM GAMEOBJECT
    [SerializeField] GameObject lockonbeamGO;
    GameObject lockonbeam;
    //THE HITBEAM GAMEOBJECT
    [SerializeField] GameObject hitbeamGO;
    GameObject hitbeam;
    //THE PIVOTPOINT GAMEOBJECT
    [SerializeField] GameObject pivotpointGO;
    GameObject pivotpoint;

    //DETERMINE WHETHER THE ENEMY STARTS SHOOTING BEAM
    bool beam_mode;
    //DISTANCE BETWEEN ENEMY AND PLAYER
    float dist;

    float timer;

    


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


        enemyScript.set_enemyType(EnemyScript.EnemyType.RANGER);


        lockonbeam = null;
        hitbeam = null;
        pivotpoint = null;
        beam_mode = false;
        timer = 0;
        hitbox.GetComponent<BoxCollider>().enabled = false;

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

        float hitbeamsize = 8;
        if (enemyScript.getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_1:
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        GetComponent<BoxCollider>().enabled = true;

                        
                        if (dist <= 2.0f)
                        {
                            beam_mode = true;
                        }
                        //CHASE THE PLAYER
                        else if (dist > 2.0f
                            && beam_mode == false)
                        {
                            navmeshagent.enabled = true;
                            navmeshagent.SetDestination(player.transform.position);
                            anim.SetBool("run", true);
                            enemyScript.setnavmeshspeed(2.0f);
                        }
                        //

                        if (beam_mode == true)
                        {
                            anim.SetBool("run", false);
                            navmeshagent.enabled = false;
                            timer += Time.deltaTime;
                            enemyScript.setnavmeshspeed(0.0f);

                            //PLACE LOCK ON BEAM
                            if (timer < 1.1f && timer > 1.0f)
                            {
                                if (pivotpoint == null)
                                {
                                    pivotpoint = Instantiate(pivotpointGO,
                                       transform.position,
                                       Quaternion.Euler(0, 0, 0));
                                    pivotpoint.transform.SetParent(
                                        GetComponentInChildren<SpriteRenderer>().transform);

                                    if (lockonbeam == null
                                        && pivotpoint != null)
                                    {
                                        lockonbeam = Instantiate(lockonbeamGO,
                                        transform.position,
                                        Quaternion.Euler(0, 0, 0));
                                        lockonbeam.transform.localScale =
                                            new Vector3(.05f,
                                            lockonbeamGO.transform.localScale.y,
                                            hitbeamsize / 10);
                                        lockonbeam.transform.SetParent(pivotpoint.transform);
                                        pivotpoint.transform.LookAt(
                                            new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                                    }
                                }
                            }
                            //

                            if (lockonbeam != null)
                            {
                                lockonbeam.transform.localScale = new Vector3(.05f / (timer * 3),
                                            lockonbeamGO.transform.localScale.y,
                                            hitbeamsize / 10);
                            }

                            if (timer < 1.9f
                                && timer >= 1.1f)
                            {
                                if (pivotpoint != null)
                                {
                                    pivotpoint.transform.LookAt(
                                         new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                                }
                            }

                            //PLACE HIT BEAM/LASER
                            if (timer > 2.3f && timer < 2.7f)
                            {
                                lockonbeam.GetComponentInChildren<Transform>().GetComponentInChildren<SpriteRenderer>().enabled = false;
                                GetComponentInChildren<SpriteRenderer>().color = Color.white;

                                anim.SetBool("run", true);
                                anim.SetBool("attack", true);

                                if (hitbeam == null
                                    && lockonbeam != null
                                    && pivotpoint != null)
                                {
                                    hitbeam = Instantiate(hitbeamGO,
                                       transform.position,
                                       Quaternion.Euler(0, 0, 0));

                                    hitbeam.transform.localScale =
                                        new Vector3(hitbeamGO.transform.localScale.x,
                                        hitbeamGO.transform.localScale.y,
                                        hitbeamsize);

                                    hitbeam.transform.SetParent(pivotpoint.transform);
                                    hitbeam.transform.rotation = pivotpoint.transform.rotation;
                                }
                            }
                            //  

                            if (timer > 2.8f)
                            {
                                navmeshagent.enabled = true;
                                anim.SetBool("attack", false);
                                em.setupdating(false);
                                DestroyBeams();
                                timer = 0.0f;
                                enemyScript.set_current_phase(EnemyScript.Phases.COOLDOWN);
                            }
                        }
                        break;
                    }
                    
                case EnemyScript.Phases.COOLDOWN:
                    {
                        beam_mode = false;
                        timer = 0.0f;
                        enemyScript.setnavmeshspeed(0.0f);

                        enemyScript.cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        beam_mode = false;
                        enemyScript.setnavmeshspeed(2.0f);
                        enemyScript.abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            beam_mode = false;
            timer = 0.0f;
            anim.SetBool("run", false);
            anim.SetBool("attack", false);
            enemyScript.ifUpdatingfalse();
            DestroyBeams();
        }
    }


    
    public void DestroyBeams()
    {
        if (hitbeam != null)
        {
            Destroy(hitbeam);
        }
        if (lockonbeam != null)
        {
            Destroy(lockonbeam);
        }
        if (pivotpoint != null)
        {
            Destroy(pivotpoint);
        }
    }
}
