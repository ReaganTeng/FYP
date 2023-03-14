using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaserScript : MonoBehaviour
{
    public float targetVelocity = 10.0f;
    public int numberOfRays = 30;
    public float angle = 90;
    public float rayRange = 5;
    public LayerMask lm;

    /*var rotation ;
    var rotationMod ;
    var direction;
    var ray;
    var deltaPosition;*/

    [SerializeField] NavMeshAgent navMeshAgent;

    private GameObject playerGO;


    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");

        //gameObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        float chasingspeed = 8.0f;
        navMeshAgent.speed = chasingspeed;
        navMeshAgent.acceleration = chasingspeed;

    }

    // Update is called once per frame
    void Update()
    {

        steering();

        //this.transform.position += deltaPosition * Time.deltaTime;
        navMeshAgent.SetDestination(playerGO.transform.position);


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
    }

    void OnDrawGizmos()
    {
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
