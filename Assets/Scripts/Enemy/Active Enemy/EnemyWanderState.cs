
using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    public override void EnterState(ActiveEnemyStateManager data)
    {
        //Debug.Log("Wander State Entered");
        data.spotProgress = 0;
        data.playerSeen = false;
        data.agent.destination = data.wanderPoints[data.currentPoint].position;
        data.agent.speed = data.baseEnemySpeed;
    }

    public override void ExitState(ActiveEnemyStateManager data)
    {
        
    }

    public override void UpdateState(ActiveEnemyStateManager data)
    {
        if (data.transform.position.x > data.wanderPoints[data.currentPoint].position.x - 0.0001 && data.transform.position.x < data.wanderPoints[data.currentPoint].position.x + 0.0001)
        {
            //Debug.Log("Path target changed!");
            ++data.currentPoint;
            if (data.currentPoint > data.wanderPoints.Count-1)
                data.currentPoint = 0;
            data.agent.destination = data.wanderPoints[data.currentPoint].position;
        }
        CheckPlayerSeen(data);
    }

    public override void OnCollisionEnter(ActiveEnemyStateManager data, Collision collision)
    {
        if(collision.transform.CompareTag("light"))
        {
            data.SwitchState(data.chaseState);
        }
    }

    public override void OnCollisionExit(ActiveEnemyStateManager data, Collision collision)
    {

    }

public override void OnTriggerStay(ActiveEnemyStateManager data, Collider collision) {}

    public override void OnTriggerEnter(ActiveEnemyStateManager data, Collider collision)
    {
        if (collision.transform.CompareTag("light"))
        {
            //switch movement sound
            data.source.Stop();
            data.source.clip = data.chasingSound;
            data.source.Play();
            AudioManager.Get.Play("midle_monster_scream", data.transform);
            data.SwitchState(data.chaseState);
        }
    }

    public override void OnTriggerExit(ActiveEnemyStateManager data, Collider collision)
    {

    }

    public void CheckPlayerSeen(ActiveEnemyStateManager data)
    {
        if (data.playerSeen)
        {
            AudioManager.Get.Play("midle_monster_scream", data.transform);
            data.SwitchState(data.spotState);
        }
    }
}
