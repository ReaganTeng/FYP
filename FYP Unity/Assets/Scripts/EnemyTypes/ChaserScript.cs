using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaserScript : MonoBehaviour
{
    public float targetVelocity = 10.0f;
    public int numberOfRays = 30;
    public float angle = 90;
    public float rayRange = 0.1f;
    public LayerMask lm;


    public GameObject lockon;
    public GameObject hit;

    /*var rotation ;
    var rotationMod ;
    var direction;
    var ray;
    var deltaPosition;*/

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


    private GameObject lockonbeam;
    private GameObject hitbeam;
    public GameObject pivotpoint;
    private GameObject pivot;

    // Start is called before the first frame update
    void Start()
    {
        lockonbeam = null;
        hitbeam = null;
        pivot = null;

        time_att_1 = 0;
        time_att_2 = 0;
        playerGO = GameObject.FindGameObjectWithTag("Player");

        //gameObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        chasingspeed = 8.0f;
        navMeshAgent.speed = chasingspeed;
        navMeshAgent.acceleration = chasingspeed;

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
        float distance = Vector3.Distance(playerGO.transform.position, transform.position);


        switch (enemyPhase)
        {
            case EnemyScript.Phases.ATTACK_TYPE_2:
                {
                    time_att_1 = 0;
                    if (distance <= 5.0f)
                    {
                        time_att_2 += 1 * Time.deltaTime;
                        chasingspeed = 0.0f;

                        if (time_att_2 < 5.1f && time_att_2 > 5.0f)
                        {
                            starting_location = transform;
                            ending_location = playerGO.transform;
                            dist = Vector3.Distance(starting_location.position, ending_location.position);

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
                                    dist / 2);
                                lockonbeam.transform.localScale +=
                                    new Vector3(0.0f, 0.0f, 1.0f)
                                    * (dist * 0.1f);
                                lockonbeam.transform.SetParent(pivot.transform);

                                pivot.transform.LookAt(ending_location);

                                pivot.transform.SetParent(transform);
                            }
                        }


                        if (time_att_2 > 6.3f)
                        {
                            if (hitbeam == null)
                            {
                                hitbeam = Instantiate(hit,
                                    lockonbeam.transform.position,
                                    lockonbeam.transform.rotation
                                    );
                                hitbeam.transform.localScale += 
                                    new Vector3(0.0f, 0.0f, 1.0f) * dist;
                                hitbeam.transform.SetParent(transform);
                            }
                        }

                        if (time_att_2 > 8.5f)
                        {
                            Destroy(lockonbeam);
                            Destroy(hitbeam);
                            Destroy(pivot);
                            GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
                        }
                    }
                    else
                    {
                        GetComponent<EnemyScript>().phase = EnemyScript.Phases.ATTACK_TYPE_1;
                    }
                    break;
                }
            case EnemyScript.Phases.ATTACK_TYPE_1:
                {
                    time_att_2 = 0;
                    time_att_1 += 1 * Time.deltaTime;
                    navMeshAgent.SetDestination(playerGO.transform.position);

                    if(time_att_1 > 20.0f)
                    {
                        GetComponent<EnemyScript>().phase = EnemyScript.Phases.COOLDOWN;
                    }
                    break;
                }
        }
    

        steering();

        
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

    void steering()
    {
        var deltaPosition = Vector3.zero;
        for (int i = 0; i < numberOfRays; i++)
        {
            //rotate enemy angle
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis(
                 (i / ((float)numberOfRays - 1)) * angle * 2 - angle,
                 this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            var direction2 = rotation * rotationMod * Vector3.back;

            var ray = new Ray(this.transform.position, direction);
            var ray2 = new Ray(this.transform.position, direction2);

            RaycastHit hitInfo;
            //if hits something
            if (Physics.Raycast(ray, out hitInfo, rayRange
                , lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
                this.transform.position += deltaPosition * Time.deltaTime;
               
            }
            else if(Physics.Raycast(ray2, out hitInfo, rayRange
                , lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction2;
                this.transform.position += deltaPosition * Time.deltaTime;

            }

        }


        float distance = Vector3.Distance(playerGO.transform.position, transform.position);

        if (distance < 5.0f)
        {
            navMeshAgent.speed = 0.0f;
            //box.size = new Vector3(box.size.x, box.size.y, box.size.z + 10.0f);
        }

        if (distance < 5.0f)
        {
            box.size = new Vector3(box.size.x, box.size.y, 4.0f);
        }
        else {
            box.size = new Vector3(box.size.x, box.size.y, 1.0f);

        }


        //var boxcollider = box;
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(boxcollider.center, boxcollider.size);
        //var boxcollider = box;

        //Debug.DrawLine(boxcollider.center, boxcollider.size);



        //OnDrawGizmos();
    }

    void OnDrawGizmos()
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



    }
}
