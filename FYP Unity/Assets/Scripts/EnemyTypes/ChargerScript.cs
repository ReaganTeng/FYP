using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChargerScript : MonoBehaviour
{
    private float chargecooldown;
    private bool collided;

    private GameObject playerGO;
    Vector3 playerPos;
    Vector3 resultingVector;

    [SerializeField] NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        chargecooldown = 4.0f;
        navMeshAgent.enabled = false;

        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerGO.transform.position;
        
        resultingVector = playerPos - gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //KEEP GOING FORWARD UNTIL HITS WALL
        if (collided)
        {

            recovering();
        }
        else
        {
            chargeAtplayer();
        }

        //Vector3 direction = playerGO.transform.position - gameObject.transform.position;
        //direction.x = -90;
        //gameObject.transform.rotation = Quaternion.LookRotation(direction);
        //Debug.Log("ROTATION: " + gameObject.transform.rotation);
    }


    public void chargeAtplayer()
    {
        Debug.Log("CHARGING");
        //resulting vector.y = 0
        resultingVector.y = 0;
        //normalise resulting vector
        resultingVector.Normalize();
        //CHARGE TOWARDS THE PLAYER'S OVERALL DIRECTION, velocity = resulting vector
        gameObject.GetComponent<Rigidbody>().velocity = /*new Vector3(70, 0, 0)*/ resultingVector* 70.0f;
    }


    public void recovering()
    {
        Debug.Log("RECOVERING");

        chargecooldown -= 1.0f * Time.deltaTime;

        if (chargecooldown <= 0.0f)
        {
            chargecooldown = 4.1f;
            //enemy.gameObject.transform.LookAt(enemy.playerGO.transform);

            //grab player location
            playerPos = playerGO.transform.position;
            //subtract between player.transform.psoition and enemy.transform.position
            resultingVector = playerPos - gameObject.transform.position;

            collided = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall"
           //&& collision.collider.transform == gameObject.transform
           )
        {
            //Debug.Log("COLLIDED WALL");

            collided = true;
        }
    }
}
