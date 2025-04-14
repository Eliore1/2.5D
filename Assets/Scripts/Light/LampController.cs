using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampController : MonoBehaviour
{
    public Light lampLight;
    public Light auraLight;
    public Transform auraLightCollider;

    public float lampLightBaseRange = 9f;
    public float auraLightBaseRange = 10f;

    float lampRange;
    float targetLampRange;
    float auraRange;
    float targetAuraRange;

    public float targetRangeReachSpeed = 2f;

    public float auraMovingSizeMultiplier = 1.75f;

    public float auraLampAimingSizeMultiplier = 0.5f;

    public float auraLampBackgroundAimingSizeMultiplier = 0.25f;

    public float auraCrouchingAndImmobileMultiplier = 0.12f;

    bool lookingAtBackground = false;
    bool looking = false;
    Vector2 movement;

    PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Get.playerManager;
        lampRange = lampLightBaseRange;
        auraRange = auraLightBaseRange;
        lampLight.range = lampRange;
        auraLight.range = auraRange;
        StartCoroutine(LightSizeUpdate(0.2f));

    }

    private void Update()
    {
        if (lampRange != targetLampRange || auraRange != targetAuraRange)
        {
            if (targetLampRange < lampRange) // if the target is a value inferior to the current value
            {
                if (lampRange - targetRangeReachSpeed * Time.deltaTime < targetLampRange) // clamp if the result will be smaller than the aimed one
                    lampRange = targetLampRange;
                else
                    lampRange -= targetRangeReachSpeed * Time.deltaTime;
            }
            else // if the target is a value superior to the current value
            {
                if (lampRange + targetRangeReachSpeed * Time.deltaTime > targetLampRange) // clamp if the result will be greater than the aimed one
                    lampRange = targetLampRange;
                else
                    lampRange += targetRangeReachSpeed * Time.deltaTime;
            }

            if (targetAuraRange < auraRange)
            {
                if (auraRange - targetRangeReachSpeed * Time.deltaTime < targetAuraRange)
                    auraRange = targetAuraRange;
                else
                    auraRange -= targetRangeReachSpeed * Time.deltaTime;
            }
            else
            {
                if (auraRange + targetRangeReachSpeed * Time.deltaTime > targetAuraRange)
                    auraRange = targetAuraRange;
                else
                    auraRange += targetRangeReachSpeed * Time.deltaTime;
            }
        }

        lampLight.range = lampRange;
        auraLight.range = auraRange;
    }

    IEnumerator LightSizeUpdate(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            targetLampRange = lampLightBaseRange;
            targetAuraRange = auraLightBaseRange;
            if (movement.x >= 0.2 || movement.x <= -0.2) // moving
                targetAuraRange *= auraMovingSizeMultiplier;
            if(movement.y < -0.5f) // crouching
                targetAuraRange *= auraCrouchingAndImmobileMultiplier;
            if (lookingAtBackground)
                targetAuraRange *= auraLampBackgroundAimingSizeMultiplier;
            else if (looking)
            {
                lampRange = 9f;
                targetLampRange = 9f;
                targetAuraRange *= auraLampAimingSizeMultiplier;
            }
            if (player.hiding)
            {
                targetAuraRange = 2;
                targetLampRange = 0;
            }

            if (!looking && !lookingAtBackground)
            {
                lampRange = 0;
                targetLampRange = 0;
            }
        }
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void LookAtInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != Vector2.zero)
            looking = true;
        else
            looking = false;
    }

    public void AimAtBackGround(InputAction.CallbackContext context)
    {
        if (context.started)
            lookingAtBackground = true;
        if (context.canceled)
            lookingAtBackground = false;
        //if (lookingAtBackground)
        //{
        //    Color playerColor = GetComponent<MeshRenderer>().material.color;
        //    playerColor.a = 0f;
        //    GetComponent<MeshRenderer>().material.SetColor("_Color", playerColor);
        //}
        //else
        //{
        //    Color playerColor = GetComponent<MeshRenderer>().material.color;
        //    playerColor.a = 1f;
        //    GetComponent<MeshRenderer>().material.SetColor("_Color", playerColor);
        //}
    }


}
