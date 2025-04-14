using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Threadmill : MonoBehaviour
{

    public enum State
    {
        STOP,
        LEFT,   
        RIGHT
    }

    [SerializeField] public Lever button;
    [SerializeField] public float speed;
    [HideInInspector] public State status;

    void Start()
    {
        status = State.STOP;
    }

    void Update()
    {
        UpdateState();
    }

    public void MoveObject(GameObject obj)
    {
        Vector3 dir = Vector3.zero;
        if (status == State.LEFT)
        {
            dir = -this.transform.right;

        }
        else
        {
            dir = this.transform.right;

        }
        if (obj.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.AddForce(dir * 50f);
        }

    }

    public void UpdateState()
    {
        if (button.status == Lever.LeverState.MIDDLE)
            status = State.STOP;
        else if (button.status == Lever.LeverState.RIGHT)
            status = State.RIGHT;
        else
            status = State.LEFT;
    }

    public void OnTriggerStay(Collider other)
    {
        if (status != State.STOP)
            MoveObject(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }
}
