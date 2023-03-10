using UnityEngine;

public abstract class StateBase
{
    public abstract void EnterState(StateManager enemy);

    public abstract void UpdateState(StateManager enemy);

    public abstract void OnCollisionEnter(StateManager enemy, Collision collision);
}
