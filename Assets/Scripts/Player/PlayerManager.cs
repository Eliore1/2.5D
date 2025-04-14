using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] public PlayerCamera playerCamera;
    [SerializeField] public LampLookController lampController;
    [SerializeField] public Movement playerMovement;
    [HideInInspector] public bool isCarrying = false;
    FxManager fx_m;
    public bool useStair = false;
    public bool checkInput = false;
    public float maxHealth = 3f;
    public float life = 10f;
    public bool hiding = false;
    CharacterController controller;
    [HideInInspector] public bool lookingAtBackground = false;
    public Transform LastCheckPoint;

    SkinnedMeshRenderer playerRenderer;
    public float iframes = 0;
    public float iframesDuration = 5;

    [HideInInspector]
    public PushAndPull carriedBox;
    void Start()
    {
        fx_m = GameManager.Get.fxmanager;
        playerRenderer = transform.Find("Player Model").Find("CHARACTER_RIG_01").GetComponent<SkinnedMeshRenderer>();
        life = maxHealth;
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (life <= 0)
            ReSpawn();
        Debug.Log(maxHealth);
        Debug.Log(life);
    }

    IEnumerator IFrameFX(float duration)
    {
        for (float i = 0; i < (duration / 0.2f) - duration / duration; i += (duration / duration))
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
            if (LastCheckPoint != null && LastCheckPoint != other.gameObject.transform)
            {
                LastCheckPoint.Find("Flag").GetComponent<Renderer>().material.color = Color.red;
            }
            LastCheckPoint = other.gameObject.transform;
            LastCheckPoint.Find("Flag").GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (iframes < Time.realtimeSinceStartup)
        {
            iframes = Time.realtimeSinceStartup + iframesDuration;
            life-=dmg;
            fx_m.InstanceEffect("BasicHit", transform.position);
            if (life > 0)
            {
                AudioManager.Get.Play("hero_hit", transform);
                StartCoroutine(IFrameFX(iframesDuration));
            }
        }
    }

    public void ReSpawn()
    {
        AudioManager.Get.Play("hero_die", transform);
        life = maxHealth;
        controller.enabled = false;
        if (LastCheckPoint != null)
        {
            transform.position = LastCheckPoint.position;
        }
        else
        {
            transform.position = new Vector3(0, 1, 0);
        }
        
        controller.enabled = true;

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            Stairs stairs = other.gameObject.GetComponent<Stairs>();
            checkInput = true;
            if (stairs.use)
                stairs.TeleportTo();
            useStair = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        checkInput = false;
        useStair = false;
    }
}
