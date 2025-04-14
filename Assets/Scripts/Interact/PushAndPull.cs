using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PushAndPull : MonoBehaviour
{

    private Interaction interactScript;
    BoxCollider bc;
    PlayerManager playerManager;
    CharacterController cc;
    Rigidbody rb;
    bool isGrabing = false;
    bool onGround = true;
    private bool isOnRealGround;
    public LayerMask ignore;




    // Start is called before the first frame update
    void Start()
    {
        interactScript = GetComponent<Interaction>();
        interactScript.Press.AddListener(TestPress);
        bc = transform.parent.GetComponent<BoxCollider>();
        playerManager = FindObjectOfType<PlayerManager>();
        cc = playerManager.GetComponent<CharacterController>();
        rb = transform.parent.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        if (playerManager.useStair)
            TestRelease();
        if (isOnRealGround && !isGrabing)
            CheckPosition();
    }

    public void CheckPosition()
    {
        if (transform.parent.position.z != 0)
        {
            Vector3 t = transform.parent.position;
            t.z = 0f;
            transform.parent.position = t;
        }
    }

    public void TestPress()
    {
        if (isGrabing)
            TestRelease();
        else
        {
            if ((playerManager.transform.rotation.eulerAngles.y == 0 || playerManager.transform.rotation.eulerAngles.y == 180) && !playerManager.playerMovement.crouching && !playerManager.useStair)
            {
                CheckPosition();
                bc.transform.parent = playerManager.transform;
                cc.radius = 1.5f;
                cc.center = new Vector3(0, 0.5f, 0);
                isGrabing = true;
                playerManager.carriedBox = this;
                playerManager.isCarrying = true;
                rb.isKinematic = true;
                rb.useGravity = false;
                transform.parent.localPosition = new Vector3(1f, -0.2f, 0f);
            }
        }
    }

    void TestRelease()
    {
        if (onGround && !playerManager.playerMovement.crouching && !playerManager.useStair)
        {
            if (playerManager.transform.eulerAngles.y != 0 || playerManager.transform.eulerAngles.y != 180)
            {
                
                if (playerManager.transform.rotation.eulerAngles.y < 90)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(playerManager.transform.position, Vector3.right, out hit, 1.8f,~ignore))
                    {
                        
                        bc.transform.parent = null;
                        bc.transform.position = playerManager.transform.position + new Vector3(1.5f, 0, 0);
                        bc.transform.eulerAngles = new Vector3(0, 0, 0);
                        cc.radius = 0.5f;
                        cc.center = new Vector3(0, 0, 0);
                        isGrabing = false;
                        playerManager.isCarrying = false;
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        CheckPosition();
                    }

                }
                else
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(playerManager.transform.position, -Vector3.right, out hit, 1.8f, ~ignore))
                    {
                        bc.transform.parent = null;
                        bc.transform.position = playerManager.transform.position + new Vector3(-1.5f, 0, 0);
                        bc.transform.eulerAngles = new Vector3(0, 0, 0);
                        cc.radius = 0.5f;
                        cc.center = new Vector3(0, 0, 0);
                        isGrabing = false;
                        playerManager.isCarrying = false;
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        CheckPosition();
                    }
                }
            }
        }
    }

    public void IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(bc.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (!onGround)
                onGround = true;    
        }
        else
            onGround = false;
    }

    public void IsOnRealGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(bc.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                if (isOnRealGround== false)
                    isOnRealGround = true;
            }
            else
            {
                if (isOnRealGround == true)
                    isOnRealGround = false;
            }
        }
    }

}
