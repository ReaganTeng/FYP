using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject Player;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] public float DetectionRange;
    float ClampedYPos;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent.updateRotation = false;
        ClampedYPos = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Playerpos = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        Vector3 Enemypos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

        float distanceBetweenPlayer = Vector3.Distance(Playerpos, Enemypos);

       /* if (distanceBetweenPlayer < DetectionRange)
        {
            navMeshAgent.SetDestination(Player.transform.position);
            Vector3 thenewpos = new Vector3(navMeshAgent.transform.position.x, ClampedYPos, navMeshAgent.transform.position.z);
            navMeshAgent.transform.position = thenewpos;
        }*/
    }
}
