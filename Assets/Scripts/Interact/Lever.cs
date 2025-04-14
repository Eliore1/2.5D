using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{

    public enum LeverState
    {
        MIDDLE,
        LEFT,
        RIGHT
    }
    [SerializeField] Interaction interact;
    [HideInInspector] public LeverState status;
    [SerializeField] public GameObject left;
    [SerializeField] public GameObject middle;
    [SerializeField] public GameObject right;
    void Start()
    {
        status  = LeverState.MIDDLE;
        interact.Press.AddListener(UseLever);
    }

    public void Update()
    {
        if (status == LeverState.MIDDLE)
        {
            middle.SetActive(true);
            left.SetActive(false);
            right.SetActive(false);
        }
        else if (status == LeverState.LEFT)
        {
            left.SetActive(true);
            middle.SetActive(false);
            right.SetActive(false);
        }
        else
        {
            right.SetActive(right);
            left.SetActive(false);
            middle.SetActive(false);
        }
    }

    public void UseLever()
    {
        AudioManager.Get.Play("lever", transform);
        if (status == LeverState.MIDDLE)
            status = LeverState.RIGHT;
        else if (status == LeverState.RIGHT)
            status = LeverState.LEFT;
        else
            status = LeverState.MIDDLE;
    }
}
