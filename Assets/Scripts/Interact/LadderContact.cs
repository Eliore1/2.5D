using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderContact : MonoBehaviour
{

    [SerializeField] bool top = false;
    Ladder LadderParent;

    private void Start()
    {
        LadderParent = transform.parent.GetComponent<Ladder>();
    }

    private void Update()
    {
        if (transform.parent.GetComponent<Ladder>().started == false)
        {
            //transform.parent.GetComponent<Ladder>().activated = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {


            if (other.gameObject.GetComponent<Movement>().climbing)
            {
                if (LadderParent.activated)
                {
                    other.gameObject.GetComponent<Movement>().climbing = false;
                    LadderParent.activated = false;
                    LadderParent.cd = Time.realtimeSinceStartup + .3f;
                    LadderParent.started = false;

                    if (top)
                    {
                        other.gameObject.GetComponent<Movement>().AddForce(new Vector3(LadderParent.JumpForce.x * LadderParent.JumpDirection, LadderParent.JumpForce.y, 0));
                    }
                } 
                

                    
            }

            if (top)
            {
                LadderParent.inTop = true;
            }
            else
            {
                LadderParent.inBot = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Movement>().climbing)
            {
                if (top && other.gameObject.transform.position.y > transform.position.y)
                {
                    other.gameObject.GetComponent<Movement>().climbing = false;
                    LadderParent.activated = false;
                    LadderParent.started = false;
                    LadderParent.cd = Time.realtimeSinceStartup + .3f;
                    other.gameObject.GetComponent<Movement>().AddForce(new Vector3(LadderParent.JumpForce.x * LadderParent.JumpDirection, LadderParent.JumpForce.y/2, 0));
                }
                else
                {
                    LadderParent.activated = true;
                }
                
            }

            if (top)
            {
                LadderParent.inTop = false;
            }
            else
            {
                LadderParent.inBot = false;
            }
        }
    }

}

