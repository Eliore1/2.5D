using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGrounded : MonoBehaviour
{

    [HideInInspector] public bool onGround = true;
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(onGround);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            onGround = true;

    }


    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            onGround = false;

    }

}
