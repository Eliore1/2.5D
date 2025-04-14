using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyInactiveState : StaticEnemyBaseState
{
    public override void EnterState(StaticEnemyStateManager data)
    {
        data.animator.SetBool("hidden", true);
        Debug.Log("Inactive State Entered!");
    }

    public override void ExitState(StaticEnemyStateManager data)
    {

    }

    public override void UpdateState(StaticEnemyStateManager data)
    {
        if(data.active) { data.SwitchState(data.shownState); }
    }
    public override void OnTriggerEnter(StaticEnemyStateManager data, Collider collision){}
    public override void OnTriggerExit(StaticEnemyStateManager data, Collider collision) { }
}
