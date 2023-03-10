using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChaser : StateBase
{
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("CHASER");

        enemy.navMeshAgent.speed = 2.0f;
    }

    public override void UpdateState(StateManager enemy)
    {
        //CONTANTLY SET DESTINATION AS PLAYER'S CURRENT POSITION
        enemy.navMeshAgent.SetDestination(enemy.playerGO.transform.position);
    }

    public override void OnCollisionEnter(StateManager enemy, Collision collision)
    {

    }
}
