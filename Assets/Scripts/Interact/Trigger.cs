using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool triggerOnce = false;
    bool triggered = false;

    public UnityEvent trigger;

    public List<string> onTriggerEnterTag;
    public List<UnityEvent> onTriggerEnter;

    public List<string> onTriggerExitTag;
    public List<UnityEvent> onTriggerExit;

    public bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if(triggerOnce && triggered) { return; }
        if(onTriggerEnter.Count == 1 && onTriggerEnterTag.Count == 0)
        {
            onTriggerEnter[0].Invoke();
            return;
        }
        if(onTriggerEnter.Count != onTriggerEnterTag.Count)
        {
            Debug.LogError("On trigger enter events dont match tags!");
            return;
        }
        for(int i = 0; i < onTriggerEnter.Count; ++i)
        {
            if (other.CompareTag(onTriggerEnterTag[i]))
            {
                triggered = true;
                onTriggerEnter[i].Invoke();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnce && triggered) { return; }
        if (onTriggerExit.Count == 1 && onTriggerExitTag.Count == 0)
        {
            onTriggerExit[0].Invoke();
            return;
        }
        if (onTriggerExit.Count != onTriggerExitTag.Count)
        {
            Debug.LogError("On trigger Exit events dont match tags!");
            return;
        }
        for (int i = 0; i < onTriggerExit.Count; ++i)
        {
            if (other.CompareTag(onTriggerExitTag[i]))
            {
                triggered = true;
                onTriggerExit[i].Invoke();
            }
        }
    }
}
    