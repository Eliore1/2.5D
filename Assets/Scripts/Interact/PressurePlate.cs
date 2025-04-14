using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject nonTriggerAspect;
    [SerializeField] GameObject TriggerAspect;
    public bool isTrigger;

    void Start()
    {
        
    }

    void Update()
    {
        if (isTrigger) 
        { 
            nonTriggerAspect.SetActive(false);
            TriggerAspect.SetActive(true);
        }
        else
        {
            TriggerAspect.SetActive(false);
            nonTriggerAspect.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            AudioManager.Get.Play("ground_button_on", transform);
            isTrigger = true;
        }
        else if (other.gameObject.tag == "Player")
        {
            AudioManager.Get.Play("ground_button_on", transform);
            isTrigger = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            isTrigger = false;
            AudioManager.Get.Play("ground_button_off", transform);
        }
        else if (other.gameObject.tag == "Player")
        {
            isTrigger = false;
            AudioManager.Get.Play("ground_button_off", transform);
        }
    }
}
