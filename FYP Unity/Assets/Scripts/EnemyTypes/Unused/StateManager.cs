using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

using UnityEngine;
using System.Collections.Specialized;

public class StateManager : MonoBehaviour
{
    public StateBase currentState;
    public StateChaser Chaser = new StateChaser();
    public StateJumper Jumper = new StateJumper();
    public StateCharger Charger = new StateCharger();

    public NavMeshAgent navMeshAgent;
    public GameObject playerGO;
    public Transform orientation;

    public bool collidedwithwall;
    public Vector3 initialvelocity;

    void Awake()
    {
    }


    //FIRS UPDATE LOOP, SET TO PATROL

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        currentState = Charger;

        collidedwithwall = false;

        currentState.EnterState(this);
    }




    // Update is called once per frame
    void Update()
    {
        //ROTATE TO PLAYER'S DIRECTION
        /*var lookpos = playerGO.transform.position - gameObject.transform.position;
        lookpos.y = 0;
        var rotation = Quaternion.LookRotation(lookpos);
        gameObject.transform.rotation = Quaternion.Slerp(
           gameObject.transform.rotation, rotation, Time.deltaTime * 10
            );*/


        //gameObject.GetComponent<Rigidbody>().AddForce((orientation.forward + orientation.right));
        currentState.UpdateState(this);
    }

    public void SwitchState(StateBase state)
    {
      
        currentState = state;
        state.EnterState(this);
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall"
            //&& collision.collider.transform == gameObject.transform
           )
        {
            Debug.Log("COLLIDED WALL");

            initialvelocity = gameObject.GetComponent<Rigidbody>().velocity;

            collidedwithwall = true;
        }
    }

    
}