using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGroupManager : MonoBehaviour
{
    [HideInInspector]
    public List<ShadowBall> balls;
    public GameObject enemyPrefab;
    public float spawnDelay = 0.2f;

    public void CreateEnemyBurst(int count)
    {
        StartCoroutine("CreateEnemyCoroutine", count);
    }

    public void KillGroupMember(ShadowBall member)
    {
        balls.Remove(member);
        Destroy(member.gameObject);
        if (balls.Count == 0)
            Destroy(gameObject);
    }

    IEnumerator CreateEnemyCoroutine(int count)
    {
        for (int a = 0; a < count; ++a)
        {
            balls.Add(Instantiate(enemyPrefab, this.transform).GetComponent<ShadowBall>());
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
