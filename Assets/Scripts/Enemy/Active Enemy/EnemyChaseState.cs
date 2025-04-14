using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(ActiveEnemyStateManager data)
    {
        //Debug.Log("Chase State Entered");
        GameManager.Get.playerManager.lampController.LampTurnedOff.AddListener(data.SetSpeedToNormal);
        data.agent.speed = data.baseEnemySpeed * data.chaseStateSpeedMultiplier;
    }

    public override void ExitState(ActiveEnemyStateManager data)
    {
        GameManager.Get.playerManager.lampController.LampTurnedOff.RemoveListener(data.SetSpeedToNormal);
    }

    public override void UpdateState(ActiveEnemyStateManager data)
    {
        data.agent.destination = GameManager.Get.playerManager.transform.position;
        if (!IsPlayerInRange(data) || data.player.hiding)
        {
            //switch movement sound
            data.source.Stop();
            data.source.clip = data.wanderingSound;
            data.source.Play();
            data.SwitchState(data.wanderState);
        }
    }

    public override void OnCollisionEnter(ActiveEnemyStateManager data, Collision collision)
    {
        //if(collision.transform.CompareTag("Player"))
        //{
        //    GameManager.Get.playerManager.TakeDamage(100); //instantly kill the player
        //    data.SwitchState(data.wanderState);
        //}

    }

    public override void OnCollisionExit(ActiveEnemyStateManager data, Collision collision)
    {

    }

public override void OnTriggerStay(ActiveEnemyStateManager data, Collider collision)
{
	if (collision.transform.CompareTag("Player"))
        {
            GameManager.Get.playerManager.TakeDamage(100); //instantly kill the player
            AudioManager.Get.Play("midle_monster_scream", data.transform);  
            //switch movement sound
            data.source.Stop();
            data.source.clip = data.wanderingSound;
            data.source.Play();
            data.SwitchState(data.wanderState);
        }
}

    public override void OnTriggerEnter(ActiveEnemyStateManager data, Collider collision)
    {
        if (collision.transform.CompareTag("light"))
        {
            data.agent.speed = data.baseEnemySpeed * data.slowDownSpeedMultiplier;
        }
        else if (collision.transform.CompareTag("Player"))
        {
            GameManager.Get.playerManager.TakeDamage(100); //instantly kill the player
            AudioManager.Get.Play("midle_monster_scream", data.transform);  
            //switch movement sound
            data.source.Stop();
            data.source.clip = data.wanderingSound;
            data.source.Play();
            data.SwitchState(data.wanderState);
        }
    }

    public override void OnTriggerExit(ActiveEnemyStateManager data, Collider collision)
    {
        if (collision.transform.CompareTag("light"))
        {
            data.agent.speed = data.baseEnemySpeed;
        }
    }

    bool IsPlayerInRange(ActiveEnemyStateManager data)
    {
        if (Vector3.Distance(data.transform.position, data.player.transform.position) > data.chaseRange)
            return false;
        return true;
    }

    
}
