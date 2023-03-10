using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaserScript : MonoBehaviour
{
   

    [SerializeField] NavMeshAgent navMeshAgent;

    private GameObject playerGO;


    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");


        float chasingspeed = 8.0f;
        navMeshAgent.speed = chasingspeed;
        navMeshAgent.acceleration = chasingspeed;

    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(playerGO.transform.position);

        

    }


    
}
