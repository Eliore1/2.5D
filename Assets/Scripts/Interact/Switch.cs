using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    private GameObject ColorButton;
    private Renderer switchRenderer;
    private Transform EyePoint;
    private PlayerManager plr_M;
    private bool inBlock = false;

    public bool activated = false;
    public Color32 color;
    public LayerMask ignore;

    List<GameObject> LightContainder = new List<GameObject>();
    

    private void Start()
    {
        ColorButton = transform.Find("ColorButton").gameObject;
        EyePoint = transform.Find("EyePoint");
        plr_M = GameManager.Get.playerManager;
        switchRenderer = ColorButton.GetComponent<Renderer>();
        plr_M.lampController.LampTurnedOff.AddListener(DeActivate);

    }

    void DeActivate()
    {
        inBlock = false;
    }

    void ActivateCondition()
    {
        if (inBlock && CheckAWall() && !activated)
        {
            activated = true;
            switchRenderer.material.color = color;
            switchRenderer.material.SetColor("_EmissionColor", color);
            switchRenderer.material.EnableKeyword("_EMISSION");
        }
        else if (!inBlock || !CheckAWall() && activated)
        {
            activated = false;
            switchRenderer.material.color = color;
            switchRenderer.material.SetColor("_EmissionColor", Color.black);
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
            if(LightContainder.Count == 1)
                AudioManager.Get.Play("button_light", transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "light")
        {
            if (LightContainder.Count == 0)
            {
                inBlock = false;
            }

            if (LightContainder.Contains(other.gameObject))
            {
                LightContainder.Remove(other.gameObject);
            }
            if (LightContainder.Count == 0)
                AudioManager.Get.Play("button_light", transform);
        }
    }
}
