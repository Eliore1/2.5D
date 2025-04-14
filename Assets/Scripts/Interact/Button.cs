using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    
    private GameObject ColorButton;
    private Renderer buttonRenderer;
    private Transform EyePoint;
    private PlayerManager plr_M;
    private bool inBlock = false;
    private bool hasStarted = false;

    public bool activated = false;
    public Color32 color;
    public LayerMask ignore;

    List<GameObject> LightContainder = new List<GameObject>();
    

    private void Start()
    {
        ColorButton = transform.Find("ColorButton").gameObject;
        EyePoint = transform.Find("EyePoint");
        plr_M = FindObjectOfType<PlayerManager>();
        plr_M.lampController.LampTurnedOff.AddListener(DeActivate);
        buttonRenderer = ColorButton.GetComponent<Renderer>();
    }

    void DeActivate()
    {
        inBlock = false;
        hasStarted = false;
    }

    void ActivateCondition()
    {
        if (inBlock && CheckAWall() && !hasStarted)
        {
            hasStarted = true;
            if (activated)
            {
                activated = false;
                AudioManager.Get.Play("button_light", transform);
                buttonRenderer.material.color = color;
                buttonRenderer.material.SetColor("_EmissionColor", Color.black);

            }
            else
            {
                activated = true;
                AudioManager.Get.Play("button_light", transform);
                buttonRenderer.material.color = color;
                buttonRenderer.material.SetColor("_EmissionColor", color);
                buttonRenderer.material.EnableKeyword("_EMISSION");
            }
        }

        foreach (GameObject a in LightContainder.ToArray())
        {
            if (a.GetComponent<MeshRenderer>().enabled == false || a.transform.parent.gameObject.activeSelf == false)
            {
                LightContainder.Remove(a);
            }
        }
    }


    private void Update()
    {
        if (LightContainder.Count == 0)
        {
            inBlock = false;
            hasStarted = false;
        }
        ActivateCondition();
    }

    bool CheckAWall()
    {
        EyePoint.LookAt(plr_M.gameObject.transform);
        Debug.DrawRay(EyePoint.position, EyePoint.forward * Vector3.Distance(EyePoint.position, plr_M.gameObject.transform.position));

        RaycastHit hit;
        if (Physics.Raycast(EyePoint.position, EyePoint.forward, out hit, Vector3.Distance(EyePoint.position, plr_M.gameObject.transform.position),~ignore))
        {
            if (hit.transform.gameObject.tag != "light")
            {
                if (hit.transform.gameObject.tag == "Player" || !LightContainder.Contains(plr_M.transform.parent.Find("Spot Light (1)").Find("Cone").gameObject))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "light")
        {
            inBlock = true;
            if (!LightContainder.Contains(other.gameObject))
            {
                LightContainder.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "light")
        {
            if (LightContainder.Count == 0)
            {
                inBlock = false;
                hasStarted = false;
            }

            if (LightContainder.Contains(other.gameObject))
            {
                LightContainder.Remove(other.gameObject);
            }
        }
    }
}
