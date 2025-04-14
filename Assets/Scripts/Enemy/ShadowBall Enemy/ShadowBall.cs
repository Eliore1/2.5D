using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : MonoBehaviour
{
    public float health = 100f;
    float maxHealth;
    public float damageReceivedPerSecond = 200f;

    public float damageDealtOnHit = 0.5f;

    public Vector2 launchAtPlayerForceMultiplier = new Vector2(200f, 500f);
    public float BallAttractionForceMultiplier = 100f;

    public Vector2 launchAtPlayerDelay = new Vector2(1f, 2f);
    public float attractToBallsDelay = 0.1f;

    public float attractionRange = 2f;

    //time till behaviour is enabled. to prevent acessing list before it is created
    public float enableBehaviourDelay = 0.75f;

    public Vector2 screamInterval = new Vector2(1f, 4f);

    public LayerMask RaycastLayers; //ignore light layer

    BallGroupManager group;
    Rigidbody ballRigidbody;
    Transform playerTransform;
    BallGroupManager groupManager;
    Material ballMat;


    // Start is called before the first frame update
    void Start()
    {
        group = GetComponentInParent<BallGroupManager>();
        ballRigidbody = GetComponent<Rigidbody>();
        playerTransform = GameManager.Get.playerManager.playerMovement.gameObject.transform;
        groupManager = GetComponentInParent<BallGroupManager>();
        maxHealth = health;
        ballMat = GetComponent<Renderer>().material;
        StartCoroutine(ScreamCoroutune());
    }

    private void Update()
    {
        if (GameManager.Get.isPlaying)
        {
            if (enableBehaviourDelay > 0)
            {
                enableBehaviourDelay -= Time.deltaTime;
                if (enableBehaviourDelay < 0)
                {
                    StartCoroutine("LaunchAtPlayerCoroutine");
                    StartCoroutine("attractToNearbyBallsCoroutine");
                }
            }
            else
            {
                List<ShadowBall> group = groupManager.balls;
                foreach (ShadowBall ball in group)
                {
                    if ((ball.transform.position - transform.position).magnitude > attractionRange)
                    {
                        Debug.DrawLine(transform.position, ball.transform.position, Color.red);
                    }
                    else
                        Debug.DrawLine(transform.position, ball.transform.position, new Color(0.988f, 0.533f, 0.012f));
                }
            }
        }
    }

    public void AttractTo(Vector3 pos)
    {
        Vector3 direction = (pos - transform.position).normalized;
        ballRigidbody.AddForce(direction * BallAttractionForceMultiplier);
    }

    IEnumerator LaunchAtPlayerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(launchAtPlayerDelay.x, launchAtPlayerDelay.y));
            Vector3 playerPos = playerTransform.position;
            playerPos.y += 1f;
            Vector3 direction = (playerPos - transform.position).normalized;
            ballRigidbody.AddForce(direction * Random.Range(launchAtPlayerForceMultiplier.x, launchAtPlayerForceMultiplier.y));
            AudioManager.Get.Play("little_monster_one_move", transform);
        }
    }

    IEnumerator attractToNearbyBallsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attractToBallsDelay);
            List<ShadowBall> group = groupManager.balls;
            foreach (ShadowBall ball in group)
            {
                if ((ball.transform.position - transform.position).magnitude > attractionRange)
                {
                    ballRigidbody.AddForce((ball.transform.position - transform.position).normalized * BallAttractionForceMultiplier);
                    ball.ballRigidbody.AddForce(-(ball.transform.position - transform.position).normalized * BallAttractionForceMultiplier);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Get.playerManager.iframes < Time.realtimeSinceStartup)
            {
                group.KillGroupMember(this);
            }
            GameManager.Get.playerManager.TakeDamage(damageDealtOnHit);
            
            
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("light"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, GameManager.Get.playerManager.transform.position - transform.position, out hit, RaycastLayers))
            {
                Debug.DrawRay(transform.position, GameManager.Get.playerManager.transform.position - transform.position);
                if (hit.collider.CompareTag("Player"))
                {
                    TakeDamage(damageReceivedPerSecond * Time.deltaTime);
                }
            }
        }

        if (health <= 0)
        {
            groupManager.KillGroupMember(this);
            AudioManager.Get.Play("little_monster_die");
        }
    }

    public void TakeDamage(float count)
    {
        health -= count;
        UpdateTransparency();
    }

    public void UpdateTransparency()
    {
        Color color = ballMat.color;
        color.a = Mathf.Lerp(0, 1, health / maxHealth);
        ballMat.color = color;
    }

    IEnumerator ScreamCoroutune()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(screamInterval.x, screamInterval.y));
            GetComponent<AudioSource>().Play();
        }
    }
}
