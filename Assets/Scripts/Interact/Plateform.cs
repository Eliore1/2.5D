using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class Plateform : MonoBehaviour
{
    [HideInInspector] public bool isCalled = false;
    private Interaction interactScript;
    [SerializeField] Transform point0;
    [SerializeField] Transform point1;
    [SerializeField] float speed;
    private Transform currentTargetPoint;
    [SerializeField] GameObject button;
    private string buttonType;
    private Vector3 direction;


    private bool IsMoving = false;

    public bool IsPausing { get; private set; }
    
    void Start()
    {
        interactScript = GetComponent<Interaction>();
        interactScript.Press.AddListener(CalledMoveTo);
        currentTargetPoint = point1;
        SetupButtonType();
        direction = Vector3.zero;
    }

    private void Update()
    {
        if(IsPausing)
        {
            if (IsMoving)
            {
                CalledMoveTo();
            }
            IsArrived();
        }
        switch (buttonType)
        {
            case "button":
                if (button.GetComponent<Button>().activated)
                {
                    if (IsPausing)
                        IsPausing = false;
                    if (IsMoving)
                        IsPausing = true;
                    else
                        IsMoving  = true;
                }
                break;
            case "PressurePlate":
                if (button.GetComponent<PressurePlate>().isTrigger)
                {
                    if (IsPausing)
                    {
                        IsMoving = true;
                        IsPausing = false;
                    }
                    
                }
                else
                {
                    IsMoving = false;
                    IsPausing = true;
                }
                break;
            case "switch":
                if (button.GetComponent<Switch>().activated)
                {
                    if (IsPausing)
                    {
                        IsPausing = false;
                        IsMoving = true;
                    }
                }
                else
                {
                    if (IsMoving)
                    {
                        IsMoving = false;
                        IsPausing = true;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void CalledMoveTo()
    {
        if (currentTargetPoint == point0)
        {
            direction = point0.position - point1.position;
        }
        else if (currentTargetPoint == point1) 
        {
            direction = point1.position - point0.position;
        }
        direction.Normalize();
        direction *= speed;
        transform.position += direction * Time.deltaTime;
        
    }

    public void IsArrived()
    {
        if (currentTargetPoint == point1 && IsMoving)
        {
            if (transform.position.y >= point1.position.y) 
            {
                Release();
                currentTargetPoint = point0;
            }
            
        }
        else if (currentTargetPoint == point0 && IsMoving)
        {
            if (transform.position.y <= point0.position.y)
            {
                Release();
                currentTargetPoint = point1;
            }
        }
    }


    public void Release()
    {
        if (currentTargetPoint == point0)
        {
            direction = Vector3.zero;
            transform.position = point0.position;
        }
        else if (currentTargetPoint == point1)
        {
            direction = Vector3.zero;
            transform.position = point1.position;   
        }
        IsMoving = false;
        button.GetComponent<Button>().activated = false;
        //isCalled = false;
    }

    public void SetupButtonType()
    {
        if (button.GetComponent<Button>())
        {
            buttonType = "button";
        }
        else if (button.GetComponent<Switch>()) 
        {
            buttonType = "switch";
        }
        else if (button.GetComponent<Timer>())
        {
            buttonType = "timer";
        }
        else if (button.GetComponent<PressurePlate>())
        {
            buttonType = "PressurePlate";
        }
        else
        {
            Debug.LogError("Button not setup for plateform");
        }
    }
}
