using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{


    PlayerManager plr_m;
    MeshRenderer playerRenderer;
    public bool Invincible = false;
    float iframes = 0;
    public float iframesDuration = 5;

    // Start is called before the first frame update
    void Start()
    {
        plr_m = GetComponent<PlayerManager>();
        playerRenderer = GetComponent<MeshRenderer>();
    }



    void CheckPlayerCollision()
    {
        
        foreach (ShadowBall a in FindObjectsOfType<ShadowBall>())
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, a.gameObject.transform.position)) < 1.2)
            {
                if (iframes < Time.realtimeSinceStartup)
                {
                    a.TakeDamage(100);
                    iframes = Time.realtimeSinceStartup + iframesDuration;
                    plr_m.life--;
                    if (plr_m.life > 0)
                    {
                        StartCoroutine(IFrameFX(iframesDuration));
                    }

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerCollision();
        if (iframes < Time.realtimeSinceStartup)
        {
            Invincible = false;
        }
        else
        {
            Invincible = true;
        }
    }

    IEnumerator IFrameFX(float duration)
    {
        for (float i = 0;i < (duration/0.2f)-duration/duration; i+=(duration/duration))
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(.2f);
        }
        playerRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            if (plr_m.LastCheckPoint != null && plr_m.LastCheckPoint != other.gameObject.transform)
            {
                plr_m.LastCheckPoint.Find("Flag").GetComponent<Renderer>().material.color = Color.red;
            }
            plr_m.LastCheckPoint = other.gameObject.transform;
            plr_m.LastCheckPoint.Find("Flag").GetComponent<Renderer>().material.color = Color.green;
        }
    }


    

}
