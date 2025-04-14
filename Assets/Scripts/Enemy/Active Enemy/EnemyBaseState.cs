using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(ActiveEnemyStateManager data);

    public abstract void ExitState(ActiveEnemyStateManager data);
    public abstract void UpdateState(ActiveEnemyStateManager data);

    public abstract void OnCollisionEnter(ActiveEnemyStateManager data, Collision collision);

    public abstract void OnCollisionExit(ActiveEnemyStateManager data, Collision collision);

public abstract void OnTriggerStay(ActiveEnemyStateManager data, Collider collision);

    public abstract void OnTriggerEnter(ActiveEnemyStateManager data, Collider collision);

    public abstract void OnTriggerExit(ActiveEnemyStateManager data, Collider collision);
}
