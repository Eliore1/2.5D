using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{


    private Interaction interactScript;
    private Movement mvm;
    private PlayerManager plr_m;

    private Transform hidingPoint;
    private Animator animator;
    
    private Vector3 oldPos;
    private bool open = false;
    private float cd = 1;
    private float actualCD = 0;

    
    void Start()
    {
        animator = transform.Find("closet 1").GetComponent<Animator>();
        interactScript = transform.Find("Interact").GetComponent<Interaction>();
        interactScript.Press.AddListener(Toggle);
        hidingPoint = transform.Find("point");
        mvm = FindObjectOfType<Movement>();
        plr_m = FindObjectOfType<PlayerManager>();
    }

    IEnumerator Enter()
    {
        AudioManager.Get.Play("hero_hide_cupboard");
        yield return new WaitForSeconds(.3f);
        plr_m.hiding = true;
        mvm.controller.enabled = false;
        mvm.gameObject.transform.position = hidingPoint.position;
        mvm.controller.enabled = true;
    }

    IEnumerator Exit()
    {
        AudioManager.Get.Play("closedoor_hero_wood");
        yield return new WaitForSeconds(.3f);
        plr_m.hiding = false;
        mvm.controller.enabled = false;
        mvm.gameObject.transform.position = new Vector3(mvm.gameObject.transform.position.x,oldPos.y,oldPos.z);
        mvm.controller.enabled = true;
        mvm.immovable = false;
    }

    void Toggle()
    {
        if (actualCD < Time.realtimeSinceStartup)
        {
            actualCD = Time.realtimeSinceStartup + cd;
            open = !open;
            animator.SetTrigger("open");
            if (open)
            {
                oldPos = mvm.gameObject.transform.position;
                mvm.immovable = true;
                StartCoroutine(Enter());
            }
            else
            {
                StartCoroutine(Exit());
            }
        }
        
    }

}
