using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallSpawner : MonoBehaviour
{
    public GameObject groupToSpawn;
    public Transform spawnPoint;

    public Vector2 groupSize = new Vector2(5,5);

    public bool continuousSpawn = false;
    public float groupSpawnDelay = 5f;

    void Start()
    {
        if(continuousSpawn)
            StartCoroutine(spawnGroupCoroutine());
    }

    IEnumerator spawnGroupCoroutine()
    {
        while (continuousSpawn) 
        { 
            yield return new WaitForSeconds(groupSpawnDelay);
            SpawnGroup();
        }
    }

    public void SpawnGroup()
    {
        Instantiate(groupToSpawn, spawnPoint).GetComponent<BallGroupManager>().CreateEnemyBurst((int)Random.Range(groupSize.x, groupSize.y));
    }

    public void Enable() { continuousSpawn = true; StartCoroutine(spawnGroupCoroutine())    ; }
    public void Disable() {  continuousSpawn = false; }
}
