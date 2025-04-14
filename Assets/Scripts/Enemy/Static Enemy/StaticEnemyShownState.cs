using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyShownState : StaticEnemyBaseState
{
    public override void EnterState(StaticEnemyStateManager data)
    {
        data.animator.SetBool("hidden", false);
        //Debug.Log("Shown State Entered!");
        data.UpdateChildrenMaterialColor(new Color(0,6,191));
        foreach(Transform child in data.transform)
        {
            child.gameObject.SetActive(true);
        }
        data.timer = data.showTime;
    }

    public override void ExitState(StaticEnemyStateManager data) { }

    public override void UpdateState(StaticEnemyStateManager data) 
    {
        if (data.PlayerInRange())
        {
            data.SwitchState(data.spottingState);
        }
        data.timer -= Time.deltaTime;
        if(data.timer  <= 0 ) 
        { 
            data.SwitchState(data.hiddenState);
        }
    }
    public override void OnTriggerEnter(StaticEnemyStateManager data, Collider collision)
    {
        if(collision.transform.CompareTag("light"))
        {
            data.beingDamaged = true;
            data.animator.SetBool("beingDamaged", true);
            data.SwitchState(data.damagedState);
        }
    }

    public override void OnTriggerExit(StaticEnemyStateManager data, Collider collision)
    {
        
    }
}
