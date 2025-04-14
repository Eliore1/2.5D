using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{


    private Renderer timerRenderer;

    
    private PlayerManager plr_M;
    private bool inBlock = false;
    private float ActualTime = 0;

    List<GameObject> LightContainder = new();

    public bool activated = false;
    public float time = 5;
    public Color32 color;
    public LayerMask ignore;

    public GameObject ColorButton;
    public GameObject Chrono;
    public TextMeshPro ChronoText;
    public Transform EyePoint;

    private void Start()
    {
        ColorButton.GetComponent<Renderer>().material.color = new Color32(255, 20, 147,255);
        plr_M = GameManager.Get.playerManager;
        plr_M.lampController.LampTurnedOff.AddListener(DeActivate);
        timerRenderer = ColorButton.GetComponent<Renderer>();
    }

    void DeActivate()
    {
        inBlock = false;
    }

    void LightContainCheck()
    {
        foreach (GameObject a in LightContainder.ToArray())
        {
            if (a.GetComponent<MeshRenderer>().enabled == false || a.transform.parent.gameObject.activeSelf == false)
            {
                LightContainder.Remove(a);
            }
        }

        if (LightContainder.Count == 0)
        {
            inBlock = false;
        }
    }

    void ActivateCondition()
    {
        if (inBlock && CheckAWall() && ActualTime <= Time.realtimeSinceStartup)
        {
            GetComponent<AudioSource>().Play();
            activated = true;
            timerRenderer.material.color = color;
            timerRenderer.material.SetColor("_EmissionColor", color);
            timerRenderer.material.EnableKeyword("_EMISSION");
            ActualTime = Time.realtimeSinceStartup + time;
        }
        if (!inBlock && ActualTime <= Time.realtimeSinceStartup && activated)
        {
            GetComponent<AudioSource>().Stop();
            AudioManager.Get.Play("end_of_timer");
            activated = false;
            timerRenderer.material.color = color;
            timerRenderer.material.SetColor("_EmissionColor", Color.black);
        }
    }

    void ChronoUpdate()
    {
        if (ActualTime - Time.realtimeSinceStartup >= 0)
        {
            ChronoText.text = (Mathf.Floor(ActualTime - Time.realtimeSinceStartup)).ToString();
            ChronoText.enabled = true;
        }
        else
        {
            ChronoText.enabled = false;
        }
    }

    private void Update()
    {
        LightContainCheck();
        ChronoUpdate();
        ActivateCondition();
    }

    bool CheckAWall()
    {
        EyePoint.LookAt(plr_M.gameObject.transform);

        if (Physics.Raycast(EyePoint.position, EyePoint.forward, out RaycastHit hit, Vector3.Distance(EyePoint.position, plr_M.gameObject.transform.position), ~ignore))
        {
            if (!hit.transform.gameObject.CompareTag("light"))
            {
                if (hit.transform.gameObject.CompareTag("Player") || !LightContainder.Contains(plr_M.transform.parent.Find("Spot Light (1)").Find("Cone").gameObject))
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
        if (other.gameObject.CompareTag("light"))
        {
            inBlock = true;
            if (!LightContainder.Contains(other.gameObject))
            {
                LightContainder.Add(other.gameObject);
            }

            /*
            if (!activated)
            {
                activated = true;
                ColorButton.GetComponent<Renderer>().material.color = Color.black;
                ActualTime = Time.realtimeSinceStartup + time;
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("light"))
        {
            if (LightContainder.Count == 0)
            {
                inBlock = false;
            }

            if (LightContainder.Contains(other.gameObject))
            {
                LightContainder.Remove(other.gameObject);
            }
        }
    }
}
