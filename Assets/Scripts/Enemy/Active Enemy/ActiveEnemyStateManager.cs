using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActiveEnemyStateManager : MonoBehaviour
{
    public List<Transform> wanderPoints;

    public float chaseStateSpeedMultiplier = 1.2f;
    public float slowDownSpeedMultiplier = 0.4f;

    public float chaseRange = 12f;

    public float spotSpeed = 1f;

    public AudioClip wanderingSound;
    public AudioClip chasingSound;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public float baseEnemySpeed;
    [HideInInspector]
    public int currentPoint = 0;
    [HideInInspector]
    public NavMeshAgent agent;

    EnemyBaseState currentState;
    [HideInInspector]
    public EnemyWanderState wanderState = new EnemyWanderState();
    [HideInInspector]
    public EnemyChaseState chaseState = new EnemyChaseState();
    [HideInInspector]
    public EnemySpottingState spotState = new EnemySpottingState();

    [HideInInspector]
    public PlayerManager player;

    [HideInInspector]
    public bool playerSeen = false;

    [HideInInspector]
    public float spotProgress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        currentState = wanderState;
        baseEnemySpeed = agent.speed;
        player = GameManager.Get.playerManager;
        if (wanderPoints.Count == 0)
            Debug.LogError("No Assigned Wander Points!");
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Get.isPlaying)
        {
            currentState.UpdateState(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        currentState.OnCollisionExit(this, collision);
    }

private void OnTtriggerStay(Collider other)
{
	currentState.OnTriggerStay(this,other);
}

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(this, other);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

    public void PlayerSeen()
    {
        playerSeen = true;  
    }

    public void PlayerUnSeen()
    {
        playerSeen = false;
    }

    public void SetSpeedToNormal()
    {
        agent.speed = baseEnemySpeed;
    }
}
