using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyBeingDamagedState : StaticEnemyBaseState
{
    public override void EnterState(StaticEnemyStateManager data)
    {
        data.damagedProgress = 0;
    }

    public override void UpdateState(StaticEnemyStateManager data)
    {
        if (data.beingDamaged)
        {
            data.damagedProgress += 100 / data.damageTime * Time.deltaTime;
            if(data.damagedProgress >= 100)
            {
                data.beingDamaged = false;
                data.animator.SetBool("beingDamaged", false);
                data.TakeDamage();
                data.MovePosition();
                data.SwitchState(data.hiddenState);
                data.damagedProgress = 0f;
            }
        }
        else
        {
            data.damagedProgress -= 100 / data.damageTime * Time.deltaTime;
            if (data.damagedProgress <= 0)
            {
                data.SwitchState(data.previousState);
            }
        }
        data.animator.SetFloat("damageProgress", data.damagedProgress / 100);
    }

    public override void ExitState(StaticEnemyStateManager data)
    {
    }

    public override void OnTriggerEnter(StaticEnemyStateManager data, Collider collision)
    {
        if (collision.transform.CompareTag("light"))
        {
            data.beingDamaged = true;
            data.animator.SetBool("beingDamaged", true);
        }
    }

    public override void OnTriggerExit(StaticEnemyStateManager data, Collider collision)
    {
        if (collision.transform.CompareTag("light"))
        {
            data.beingDamaged = false;
            data.animator.SetBool("beingDamaged", false);
        }
    }
}
