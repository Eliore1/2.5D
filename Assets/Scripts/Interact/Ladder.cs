using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    bool inside;
    Movement plrScript;
    PlayerManager plr_m;
    public bool started = false;
    public bool activated = false;
    public int JumpDirection = 1;
    public float cd = 0;
    public Vector2 JumpForce;

    AudioSource ladderAudio;

    [HideInInspector] public bool inTop = false;
    [HideInInspector] public bool inBot = false;

    // Start is called before the first frame update
    void Start()
    {
        ladderAudio = GetComponent<AudioSource>();
        plrScript = FindObjectOfType<Movement>();
        plr_m = FindObjectOfType<PlayerManager>();
    }

    void Climb()
    {

        if (!plrScript.climbing)
        {

            activated = false;
            started = false;
        }

        plrScript.climbing = !plrScript.climbing;
        plrScript.controller.enabled = false;
        plrScript.gameObject.transform.position = new Vector3(transform.position.x, plrScript.transform.position.y, plrScript.transform.position.z);
        plrScript.controller.enabled = true;
        started = true;


        if (!plrScript.climbing)
        {
            
            activated = false;
            started = false;
        }
        else
        {
            plrScript.direction.y = 0;
            if (plr_m.isCarrying)
            {
                GameManager.Get.playerManager.carriedBox.TestPress();
                //plrScript.transform.Find("Box").Find("InteractionZone2").GetComponent<Interaction>().Press.Invoke();
            }
        }

        if (plrScript.transform.position.y < transform.Find("Top").position.y && plrScript.transform.position.y > transform.Find("Bot").position.y && plrScript.climbing == true && !inBot && !inTop)
        {
            activated = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inside = false;
    }


    void ChronoUpdate()
    {
        if (!plrScript.climbing && (activated || started))
        {
            cd = Time.realtimeSinceStartup + 1f;
            activated = false;
            started = false;
        }
    }

    void InputCheck()
    {
        if (inside && plrScript.plrInputs.actions["Move"].ReadValue<Vector2>().y > 0.8f && started == false && cd < Time.realtimeSinceStartup)
        {
            ladderAudio.Play();
            Climb();

        }
        else
            ladderAudio.Stop();
    }
    
    void Update()
    {
        ChronoUpdate();
        InputCheck();
    }

    
}
