using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{

    public UnityEvent Press;
    public UnityEvent Release;
    [HideInInspector] public bool used;


    public void Interact(bool release)
    {
        if (!release)
        {
            Press.Invoke();
        }
        else
        {
            Release.Invoke();
        }
    }
}
