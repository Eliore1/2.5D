using UnityEngine;

public class NormalDoor : MonoBehaviour
{

    Interaction interactScript;
    DoorKey keyScript;
    Transform Pivot;
    BoxCollider Hitbox;
    bool open = false;
    float interpolation;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] Vector3 MoveTo = new(0f, 90f, 0f);
    [SerializeField] Vector3 MoveFrom = new(0f, 0f, 0f);
    [SerializeField] GameObject key;
    Vector3 basePos;


    void Start()
    {
        if (key)
        {
            keyScript = key.GetComponent<DoorKey>();
        }
        Pivot = transform.Find("Pivot");
        interactScript = transform.Find("Interact").GetComponent<Interaction>();
        interactScript.Press.AddListener(Toggle);
        Hitbox = transform.Find("Hitbox").GetComponent<BoxCollider>();
    }

    void Toggle()
    {
        if (key)
        {
            if (keyScript.Picked)
            {

                open = !open;
                Hitbox.enabled = !open;
            }
        }
        else
        {
            open = !open;
            Hitbox.enabled = !open;
        }
        if (open)
        {
            if (key)
                AudioManager.Get.Play("open_door_key", transform);
            else
                AudioManager.Get.Play("opendoor_hero_wood", transform);
        }
        else
            AudioManager.Get.Play("closedoor_hero_wood", transform);

    }

    void InterpolationUpdate()
    {
        Pivot.transform.eulerAngles = Vector3.Lerp(basePos + MoveFrom, basePos + MoveTo, interpolation);
        if (open)
        {
            interpolation += moveSpeed * Time.smoothDeltaTime;
            interpolation = Mathf.Clamp(interpolation, 0, 1);
        }
        else
        {
            interpolation -= moveSpeed * Time.smoothDeltaTime;
            interpolation = Mathf.Clamp(interpolation, 0, 1);
        }
    }

    void Update()
    {
        InterpolationUpdate();
    }
}
