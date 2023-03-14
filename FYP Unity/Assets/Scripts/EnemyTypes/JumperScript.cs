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

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");

        jumpcooldown = 4.0f;

        timer = 0.0f;

        navMeshAgent.SetDestination(playerGO.transform.position);

        navMeshAgent.acceleration = 20.0f/* 5.0f * Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position)*/;

        //speedfactor = Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position);
        speedfactor = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {


        //if (Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position) <= 0.5f)
        if (timer >= 1.0f)
        {
            jumprest();
        }
        else
        {
            timer += 1 * Time.deltaTime;

            //while it's jumping, disable collider;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            navMeshAgent.speed = 30.0f * speedfactor;
        }
        //Debug.Log("TIMER " + timer);


    }


    public void jumprest()
    {
        jumpcooldown -= 1.0f * Time.deltaTime;

        navMeshAgent.speed = 0.0f;

        gameObject.GetComponent<BoxCollider>().enabled = true;

        if (jumpcooldown <= 0.0f)
        {
            jumpcooldown = 4.1f;
            timer = 0.0f;

            navMeshAgent.SetDestination(playerGO.transform.position);
            //speedfactor = Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position);
            speedfactor = 20.0f;
        }
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.gameObject.tag == "Player"
    //       )
    //    {
    //        Debug.Log("DAMAGED PLAYER");
    //    }
    //}

    
}
