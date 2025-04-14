using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] GameObject Button;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] Vector3 MoveTo = new(0f, 10f, 0f);
    [SerializeField] Vector3 MoveFrom = new(0f, 0f, 0f);
    public bool PlayerOn;
    GameObject DoorMovable;
    Vector3 basePos;
    bool soundPlayed = false;

    enum ButtonType
    {
        Button,
        Switch,
        Timer,
        PressurePlate
    }

    ButtonType typeButton;
    float interpolation;
    float acceleration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        DoorMovable = transform.Find("04").gameObject;
        basePos = DoorMovable.transform.position;
        DoorSetup();
    }

    void DoorSetup()
    {
        if (Button)
        {
            if (Button.GetComponent<Button>())
            {
                typeButton = ButtonType.Button;
                DoorMovable.GetComponent<Renderer>().material.color = Button.transform.Find("ColorButton").gameObject.GetComponent<Renderer>().material.color;

            }
            else if (Button.GetComponent<Switch>())
            {
                typeButton = ButtonType.Switch;
                DoorMovable.GetComponent<Renderer>().material.color = Button.transform.Find("ColorButton").gameObject.GetComponent<Renderer>().material.color;

            }
            else if (Button.GetComponent<Timer>())
            {
                typeButton = ButtonType.Timer;
                DoorMovable.GetComponent<Renderer>().material.color = Button.transform.Find("ColorButton").gameObject.GetComponent<Renderer>().material.color;

            }
            else if (Button.GetComponent<PressurePlate>())
            {
                typeButton = ButtonType.PressurePlate;
            }
            else
            {
                Debug.LogError("The " + Button.name + " GameObject does have a Button/Switch/Timer/PressurePlate script attached to it.");
            }
        }
        else
        {
            Debug.LogError("No button has been assigned to this gameObject.");
        }

    }

    bool IsActive()
    {
        return typeButton switch
        {
            (ButtonType.Button) => Button.GetComponent<Button>().activated,
            (ButtonType.Switch) => Button.GetComponent<Switch>().activated,
            (ButtonType.Timer) => Button.GetComponent<Timer>().activated,
            (ButtonType.PressurePlate) => Button.GetComponent<PressurePlate>().isTrigger,
            _ => false,
        };
    }

    void InterpolationUpdate()
    {
        if (IsActive())
        {
            if (!soundPlayed)
            {
                AudioManager.Get.Play("mecanical_door");
                soundPlayed = true;
            }
            acceleration += 0.005f;
            acceleration = Mathf.Clamp(acceleration, 0, 1);
            interpolation += moveSpeed * acceleration * Time.smoothDeltaTime;
            interpolation = Mathf.Clamp(interpolation, 0, 1);

        }
        else
        {
            soundPlayed = false;
            acceleration -= 0.005f;
            acceleration = Mathf.Clamp(acceleration, -1, 0);
            interpolation += moveSpeed * acceleration * Time.smoothDeltaTime;
            interpolation = Mathf.Clamp(interpolation, 0, 1);
        }
    }

    private void FixedUpdate()
    {
        if (PlayerOn)
            DoorMovable.transform.position = Vector3.Lerp(basePos + MoveFrom, basePos + MoveTo, interpolation);
    }

    void Update()
    {
        if (!PlayerOn)
            DoorMovable.transform.position = Vector3.Lerp(basePos + MoveFrom, basePos + MoveTo, interpolation);

        InterpolationUpdate();
        
    }


    
}
