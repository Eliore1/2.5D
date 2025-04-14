using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyHiddenState : StaticEnemyBaseState
{
    public override void EnterState(StaticEnemyStateManager data)
    {
        //Debug.Log("Hidden State Entered!");
        data.animator.SetBool("hidden", true);
        foreach (Transform child in data.transform)
        {
            child.gameObject.SetActive(false);
        }
        data.timer = data.hideTime;
    }

    public override void ExitState(StaticEnemyStateManager data)
    {

    }

    public override void UpdateState(StaticEnemyStateManager data)
    {       
        if (data.PlayerInRange())
        {
            data.SwitchState(data.spottingState);
        }
        data.timer -= Time.deltaTime;
        if (data.timer <= 0)
        {
            data.SwitchState(data.shownState);
        }
    }

    public override void OnTriggerEnter(StaticEnemyStateManager data, Collider collision)
    {

    }

    public override void OnTriggerExit(StaticEnemyStateManager data, Collider collision) { }
}
