using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemySpottingState : StaticEnemyBaseState
{
    public override void EnterState(StaticEnemyStateManager data)
    {
        data.animator.SetBool("spotting", true);
        Debug.Log("Spotting State Entered!");
        foreach (Transform child in data.transform)
        {
            child.gameObject.SetActive(true);
        }
        
    }

    public override void ExitState(StaticEnemyStateManager data)
    {
        data.animator.SetBool("spotting", false);
    }

    public override void UpdateState(StaticEnemyStateManager data)
    {
        if(data.PlayerInRange())
        {
            data.spotProgress += 100 / data.damageGracePeriod * Time.deltaTime;
            if(data.spotProgress > 100) 
            {
                AudioManager.Get.Play("big_monster_attack");
                data.player.TakeDamage(data.damageOnPlayerHit);
                data.MovePosition();
                data.spotProgress = 0;
                data.SwitchState(data.previousState);
            }
        }
        else
        {
            data.spotProgress -= 100 / data.damageGracePeriod * Time.deltaTime;
            if (data.spotProgress < 0)
            {
                data.SwitchState(data.previousState);
            }
        }
        float progress = data.spotProgress / 100;
        if(progress > 1) progress = 1;
        else if (progress < 0) progress = 0;
        data.animator.SetFloat("spotProgress", data.spotProgress/100);
        data.UpdateChildrenMaterialColor(Color.Lerp(Color.blue, Color.red, data.spotProgress / 100));
    }

    public override void OnTriggerEnter(StaticEnemyStateManager data, Collider collision)
    {

    }

    public override void OnTriggerExit(StaticEnemyStateManager data, Collider collision)
    {

    }
}
