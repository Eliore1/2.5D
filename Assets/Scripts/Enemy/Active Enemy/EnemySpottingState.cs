
using UnityEngine;

public class EnemySpottingState : EnemyBaseState
{
    public override void EnterState(ActiveEnemyStateManager data)
    {

    }

    public override void ExitState(ActiveEnemyStateManager data)
    {

    }

    public override void UpdateState(ActiveEnemyStateManager data)
    {
        if (data.playerSeen)
        {
            data.spotProgress += 100 / data.spotSpeed * Time.deltaTime;
            if (data.spotProgress > 100)
            {
                //switch movement sound
                data.source.Stop();
                data.source.clip = data.chasingSound;
                data.source.Play();
                data.SwitchState(data.chaseState);
            }
        }
        else
        {
            data.spotProgress -= 100 / data.spotSpeed * Time.deltaTime;
            if (data.spotProgress < 0)
            {
                data.SwitchState(data.wanderState);
            }
        }
    }

    public override void OnCollisionEnter(ActiveEnemyStateManager data, Collision collision)
    {

    }

    public override void OnCollisionExit(ActiveEnemyStateManager data, Collision collision)
    {

    }

public override void OnTriggerStay(ActiveEnemyStateManager data, Collider collision) {}

    public override void OnTriggerEnter(ActiveEnemyStateManager data, Collider collision)
    {

    }

    public override void OnTriggerExit(ActiveEnemyStateManager data, Collider collision)
    {

    }
}
