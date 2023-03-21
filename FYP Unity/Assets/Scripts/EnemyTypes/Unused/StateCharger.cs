using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCharger : StateBase
{
    private float chargecooldown;
    private bool collided;

    public override void EnterState(StateManager enemy)
    {
        Debug.Log("CHARGER");
        collided = false;
        chargecooldown = 4.0f;


        //var lookpos = enemy.playerGO.transform.position - enemy.gameObject.transform.position;
        //lookpos.z = 0;
        //var rotation = Quaternion.LookRotation(lookpos);
        //enemy.gameObject.transform.rotation = Quaternion.Slerp(
        //    enemy.gameObject.transform.rotation, rotation, Time.deltaTime * 1000.0f
        //    );

        //enemy.gameObject.transform.LookAt(enemy.playerGO.transform);
    }

    public override void UpdateState(StateManager enemy)
    {
        //KEEP GOING FORWARD UNTIL HITS WALL
        if(enemy.collidedwithwall)
        {

            Debug.Log("RECOVERING");

            chargecooldown -= 1.0f * Time.deltaTime;

            //LET ENEMY BACK AWAY
            if (chargecooldown > 3.1f)
            {
                
                enemy.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-10, 0, 0);
               
            }
            else
            {
                enemy.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            //

            if (chargecooldown <= 0.0f)
            {
                chargecooldown = 4.1f;
                //enemy.gameObject.transform.LookAt(enemy.playerGO.transform);



                enemy.collidedwithwall = false;
            }
        }
        else
        {
            Debug.Log("CHARGING");

            //CHARGE TOWARDS THE PLAYER'S OVERALL DIRECTION
            enemy.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(70, 0, 0);
        }

        //enemy.gameObject.transform.LookAt(enemy.playerGO.transform);

        //var lookpos = enemy.playerGO.transform.position - enemy.gameObject.transform.position;
        //lookpos.z = 0;
        //var rotation = Quaternion.LookRotation(lookpos);
        //enemy.gameObject.transform.rotation = Quaternion.Slerp(
        //    enemy.gameObject.transform.rotation, rotation, Time.deltaTime * 900.0f
        //    );

        //Vector3 direction = /*enemy.gameObject.transform.position -*/ enemy.playerGO.transform.position - enemy.gameObject.transform.position;
        //direction.x = -90;
        //enemy.gameObject.transform.rotation = Quaternion.LookRotation(direction);
        Debug.Log("ROTATION: " + enemy.gameObject.transform.rotation);
    }

    public override void OnCollisionEnter(StateManager enemy, Collision collision)
    {
        //if (collision.gameObject.tag == "wall"
        //    && collision.collider.transform == enemy.gameObject.transform
        //    && collided == false)
        //{
        //    Debug.Log("COLLIDED WALL");

        //   collided = true;
        //}
    }
}
