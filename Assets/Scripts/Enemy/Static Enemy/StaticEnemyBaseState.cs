using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticEnemyBaseState
{
    public abstract void EnterState(StaticEnemyStateManager data);

    public abstract void ExitState(StaticEnemyStateManager data);

    public abstract void UpdateState(StaticEnemyStateManager data);

    public abstract void OnTriggerEnter(StaticEnemyStateManager data, Collider collision);

    public abstract void OnTriggerExit(StaticEnemyStateManager data, Collider collision);
}
