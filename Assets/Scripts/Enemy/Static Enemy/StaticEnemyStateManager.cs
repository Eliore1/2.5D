using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyStateManager : MonoBehaviour
{
    public List<Transform> points;

    public bool active = false;
    public int health = 3;

    public float damageOnPlayerHit = 1f;
    public float damageGracePeriod = 1f;

    public float damageRange = 1f;

    public float hideTime = 7f;
    public float showTime = 3f;

    public float damageTime = 0.3f;

    [HideInInspector] public bool beingDamaged = false;
    [HideInInspector] public float damagedProgress = 0f;

    [HideInInspector] public float timer = 0f;
    public float spotProgress = 0f;

    [HideInInspector]public int currentPoint = 0;

    [HideInInspector] public PlayerManager player;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Animator animator;

    StaticEnemyBaseState currentState;
    [HideInInspector]public StaticEnemyBaseState previousState;
    [HideInInspector]public StaticEnemyHiddenState hiddenState = new StaticEnemyHiddenState();
    [HideInInspector]public StaticEnemyInactiveState inactiveState = new StaticEnemyInactiveState();
    [HideInInspector]public StaticEnemyShownState shownState = new StaticEnemyShownState();
    [HideInInspector]public StaticEnemySpottingState spottingState = new StaticEnemySpottingState();
    [HideInInspector]public StaticEnemyBeingDamagedState damagedState = new StaticEnemyBeingDamagedState();

    public List<MeshRenderer> childrenRenderers;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = points[0].position;
        player = GameManager.Get.playerManager;
        playerTransform = player.transform;
        if (active)
            currentState = shownState;
        else
            currentState = inactiveState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Get.isPlaying)
            currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(this, other);
    }

    public void SwitchState(StaticEnemyBaseState state)
    {
        previousState = currentState;
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);

    }

    void Activate() { active = true; }
    void Deactivate() { active = false; }

    public void UpdateChildrenMaterialColor(Color color)
    {
        foreach(MeshRenderer meshRenderer in childrenRenderers)
        {
            meshRenderer.material.SetColor("_EmissionColor", color);
        }
    }

    public void MovePosition()
    {
        ++currentPoint;
        if(currentPoint > points.Count-1)
        {
            currentPoint = 0;
        }
        transform.position = points[currentPoint].transform.position;
        AudioManager.Get.Play("big_monster_move-imported");
    }

    public void TakeDamage()
    {
        --health;
        AudioManager.Get.Play("big_monster_scream");
        if (health == 0)
            Destroy(GetComponentInParent<Transform>().gameObject);
    }

    public bool PlayerInRange()
    {
        if (Mathf.Abs(playerTransform.position.x - transform.position.x) < damageRange)
            return true;
        return false;
    }
}
