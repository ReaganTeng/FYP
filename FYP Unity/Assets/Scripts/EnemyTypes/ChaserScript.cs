using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaserScript : MonoBehaviour
{ 
 float targetVelocity = 10.0f;
    int numberOfRays = 30;
    float angle = 90.0f;
    float rayRange = .8f;
    [SerializeField] LayerMask lm;


public GameObject lockon;
private GameObject lockonbeam;
public GameObject hit;
private GameObject hitbeam;
public GameObject pivotpoint;
private GameObject pivot;

[SerializeField] NavMeshAgent navMeshAgent;

private GameObject playerGO;
public BoxCollider box;
float chasingspeed;
public EnemyScript.Phases enemyPhase;

private float time_att_1;
private float time_att_2;

private Transform starting_location;
private Transform ending_location;
private float dist;
bool beam_mode;

[SerializeField] GameObject attackhitbox;

    // Start is called before the first frame update
    void Awake()
    {
        lockonbeam = null;
        hitbeam = null;
        pivot = null;

        beam_mode = false;
        time_att_1 = 0;
        time_att_2 = 0;
        playerGO = GameObject.FindGameObjectWithTag("Player");

        chasingspeed = 4.0f;
        attackhitbox.SetActive(true);


        GetComponent<EnemyScript>().setabouttoattackend(3.0f);
        GetComponent<EnemyScript>().setCoolDownEnd(3.0f);
    }

    void Start()
{
    lockonbeam = null;
    hitbeam = null;
    pivot = null;

    beam_mode = false;
    time_att_1 = 0;
    time_att_2 = 0;
    playerGO = GameObject.FindGameObjectWithTag("Player");

    chasingspeed = 4.0f;
        attackhitbox.SetActive(true);


        GetComponent<EnemyScript>().setabouttoattackend(3.0f);
    GetComponent<EnemyScript>().setCoolDownEnd(3.0f);
}

// Update is called once per frame
void Update()
{


        enemyPhase = GetComponent<EnemyScript>().phase;
    playerGO = GameObject.FindGameObjectWithTag("Player");
    navMeshAgent.speed = chasingspeed;
    navMeshAgent.acceleration = chasingspeed;

    dist = Vector3.Distance(transform.position, playerGO.transform.position);


        //CHASER SHOULD BE SLOWER THAN CHARGER
        if (GetComponent<EnemyScript>().getupdating())
        {
            switch (enemyPhase)
            {
                case EnemyScript.Phases.ATTACK_TYPE_2:
                    {
                        attackhitbox.SetActive(true);

                        //GetComponent<SpriteRenderer>().enabled = false;

                        attackhitbox.GetComponent<BoxCollider>().enabled = true;
                        time_att_1 = 0;
                        if (dist <= 4.0f)
                        {
                            beam_mode = true;
                        }
                        else if (dist > 4.0f
                            && beam_mode == false)
                        {
                            GetComponent<EnemyScript>().phase = EnemyScript.Phases.ATTACK_TYPE_1;
                        }

                        if (beam_mode == true)
                        {
                            time_att_2 += 1 * Time.deltaTime;
                            chasingspeed = 0.0f;
                            if (time_att_2 < 1.1f && time_att_2 > 1.0f)
                            {
                                starting_location = transform;
                                ending_location = playerGO.transform;
                                if (lockonbeam == null)
                                {
                                    pivot = Instantiate(pivotpoint,
                                       transform.position,
                                       Quaternion.identity
                                       );

                                    lockonbeam = Instantiate(lockon,
                                        transform.position,
                                        Quaternion.identity
                                        );
                                    lockonbeam.transform.position += new Vector3(0.0f, 0.0f,
                                        4.0f / 2);
                                    lockonbeam.transform.localScale +=
                                        new Vector3(0.0f, 0.0f, 1.0f)
                                        * (4.0f * 0.1f);
                                    lockonbeam.transform.SetParent(pivot.transform);

                                    pivot.transform.LookAt(ending_location);

                                    pivot.transform.SetParent(transform);
                                }
                            }
                            if (time_att_2 > 2.3f)
                            {
                                if (hitbeam == null)
                                {
                                    hitbeam = Instantiate(hit,
                                        lockonbeam.transform.position,
                                        lockonbeam.transform.rotation
                                        );
                                    hitbeam.transform.localScale +=
                                        new Vector3(0.0f, 0.0f, 1.0f) * 4.0f;
                                    hitbeam.transform.SetParent(transform);
                                }
                            }

                            if (time_att_2 > 3.5f)
                            {
                                Destroy(lockonbeam);
                                Destroy(hitbeam);
                                Destroy(pivot);

                                GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
                            }
                        }
                        break;
                    }
                case EnemyScript.Phases.ATTACK_TYPE_1:
                    {
                        attackhitbox.SetActive(true);

                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);

                        beam_mode = false;

                        //GetComponent<SpriteRenderer>().enabled = false;

                        //Debug.Log("CHASING PLAYER");
                        attackhitbox.GetComponent<BoxCollider>().enabled = true;

                        chasingspeed = 4.0f;
                        time_att_2 = 0;
                        time_att_1 += 1 * Time.deltaTime;
                        navMeshAgent.SetDestination(playerGO.transform.position);


                        if (time_att_1 > 20.0f)
                        {
                            GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
                        }
                        break;
                    }
                case EnemyScript.Phases.COOLDOWN:
                    {
                        beam_mode = false;
                        //GetComponent<SpriteRenderer>().enabled = false;

                        time_att_2 = 0.0f;
                        time_att_1 = 0.0f;

                        GetComponent<EnemyScript>().cooldownUpdate();
                        break;
                    }
                case EnemyScript.Phases.ABOUT_TO_ATTACK:
                    {
                        beam_mode = false;

                        //GetComponent<SpriteRenderer>().enabled = true;

                        GetComponent<EnemyScript>().abouttoattackUpdate();
                        break;
                    }
            }
        }
        else
        {
            DestroyBeams();
        }

    //        steering();


        //if (attackhitbox.activeSelf == true)
        //{
        //    Debug.Log("Hitbox ACTIVE");
        //}

    /*if (time < 5.0f)
    {
        switch (enemyPhase)
        {
            case EnemyScript.Phases.PHASE_1:
                {
                    chasingspeed = 8.0f;
                    break;
                }
            //let enemy constantly chase the player after jumping
            case EnemyScript.Phases.PHASE_2:
                {
                    chasingspeed = 11.0f;
                    break;
                }
            case EnemyScript.Phases.PHASE_3:
                {
                    chasingspeed = 14.0f;
                    break;
                }
            default:
                break;
        }

        steering();

        //this.transform.position += deltaPosition * Time.deltaTime;
        navMeshAgent.SetDestination(playerGO.transform.position);
    }*/



}


    public void DestroyBeams()
    {
        if (lockonbeam != null)
        {
            Destroy(lockonbeam);
        }


        if (hitbeam != null)
        {
            Destroy(hitbeam);
        }

        if (pivot != null)
        {
            Destroy(pivot);
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
            Debug.Log("HIT SOMETHING");
            deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
            transform.position += deltaPosition * Time.deltaTime;

        }
        else if (Physics.Raycast(ray2, out hitInfo, rayRange
            , ~lm))
        {
            Debug.Log("HIT SOMETHING");

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

    /*void OnDrawGizmos()
    {
        var boxcollider = box;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxcollider.center, boxcollider.size);

        playerGO = GameObject.FindGameObjectWithTag("Player");

        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, (playerGO.transform.position - this.transform.position).normalized *
            Vector3.Distance(playerGO.transform.position, this.transform.position));

        for (int i = 0; i < numberOfRays; i++)
        {
            //rotate enemy angle
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis(
                 (i / ((float)numberOfRays - 1)) * angle * 2 - angle,
                 this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            var direction2 = rotation * rotationMod * Vector3.back;

           
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, direction);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(this.transform.position, direction2);
        }



    }*/
}
