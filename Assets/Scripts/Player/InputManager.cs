using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public UnityEvent Move;
    public UnityEvent Crouch;
    public UnityEvent Sprint;
    public UnityEvent Jump;


    private static InputManager Instance;
    public static InputManager Get
    {
        get
        {
            if (Instance == null)
            {
                Debug.LogWarning("Not instance of InputManager");
            }
            return Instance;
        }

    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void InputMove()
    {
        Move.Invoke();
    }

    public void InputJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Jump.Invoke();
        }
    }

    public void InputCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Crouch.Invoke();
        }
    }

}
