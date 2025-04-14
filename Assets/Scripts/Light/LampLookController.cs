using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class LampLookController : MonoBehaviour
{
    public LayerMask cameraRaycastLayers;
    public Camera cam;

    public float maxDistanceFromCenter = 100; //assumes 1080x1920, will scale if not
    public float lookAtBackGroundIntensityMultiplier = 1f;
    public float lookAtBackGroundMoveSpeed = 300f;
    public float turnOffDelay = 0.1f;
    Vector2 lookDirection;
    bool lookingAtBackground = false;
    Vector2 screenPos = Vector2.zero;
    
    float baseLightAngle;
    float baseLightRange;
    float baseLightIntensity;

    public Vector2 maxRotation = new(75f,75f); //checked in the positive and negative

    public UnityEvent LampTurnedOff;
    
    private void Start()
    {
        baseLightRange = GetComponent<Light>().range;
        baseLightAngle = GetComponent<Light>().spotAngle;
        baseLightIntensity = GetComponent<Light>().intensity;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(lookingAtBackground)
        {
            //Debug.Log((screenPos + lookDirection - new Vector2(Screen.width / 2, Screen.height / 2)).magnitude);
            if ((screenPos + (lookAtBackGroundMoveSpeed * Time.deltaTime * lookDirection) - new Vector2(Screen.width / 2, Screen.height / 2)).magnitude < maxDistanceFromCenter)
            {
                screenPos += lookAtBackGroundMoveSpeed * Time.deltaTime * lookDirection;
                Ray camRayCast = cam.ScreenPointToRay(screenPos);
                if (Physics.Raycast(camRayCast, out RaycastHit hit, Mathf.Infinity, cameraRaycastLayers))
                {
                    Debug.DrawRay(camRayCast.origin, hit.point - camRayCast.origin, Color.red, 1);
                    if (hit.point.z < transform.position.z) // prevent looking behind the player
                        return;
                    GetComponent<Transform>().LookAt(hit.point);

                    float range = (hit.point - transform.position).magnitude + 2f;
                    float angle = baseLightAngle / ((hit.point - transform.position).magnitude / baseLightRange);
                    float intensity = baseLightIntensity * ((hit.point - transform.position).magnitude / (baseLightRange * 3)) * lookAtBackGroundIntensityMultiplier;
                    Vector3 scale = new(1, 1, 1 / baseLightRange * range);
                    if ((hit.point - transform.position).magnitude < baseLightRange)
                    {
                        angle = baseLightAngle;
                        intensity = baseLightIntensity;
                        range = baseLightRange;
                    }
                    CheckForMaxRotation();

                    transform.localScale = scale;
                    GetComponent<Light>().range = range;
                    GetComponent<Light>().spotAngle = angle;
                    GetComponent<Light>().intensity = intensity;


                }
            }
        }
    }

    public void LookTowards(InputAction.CallbackContext context)
    {
        if (!GameManager.Get.isPlaying)
            return;
        lookDirection = context.ReadValue<Vector2>();
        if (!lookingAtBackground)
        {
            if (lookDirection == Vector2.zero)
            {
                GetComponent<AudioSource>().Stop();
                AudioManager.Get.Play("lamp_off");
                gameObject.SetActive(false);
                LampTurnedOff.Invoke();
                return;
            }
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
                GetComponent<AudioSource>().Play();
                AudioManager.Get.Play("lamp_on");
            }
            transform.rotation = Quaternion.LookRotation(new Vector3(lookDirection.x, lookDirection.y), new Vector3(0, 0, -1));
        }
    }


    public void AimAtBackGroundToggle(InputAction.CallbackContext context)
    {
        if (!GameManager.Get.isPlaying)
            return;
        if (context.started)
            lookingAtBackground = true;
        if (context.canceled)
            lookingAtBackground = false;
        GameManager.Get.playerManager.lookingAtBackground = lookingAtBackground;
        if (lookingAtBackground) // activate
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
                AudioManager.Get.Play("lamp_on");
                GetComponent<AudioSource>().Play();
            }
            screenPos = new Vector2(Screen.width / 2, Screen.height / 2);
        }
        else
        {
            GetComponent<Light>().range = baseLightRange;
            GetComponent<Light>().spotAngle = baseLightAngle;
            GetComponent<Light>().intensity = baseLightIntensity;
            transform.localScale = Vector3.one;
            LampTurnedOff.Invoke();
            gameObject.SetActive(false);
            GetComponent<AudioSource>().Stop();
            AudioManager.Get.Play("lamp_off");
        }
    }

    //public void Movement(InputAction.CallbackContext context)
    //{
    //    if(lookingAtBackground && context.ReadValue<Vector2>() != Vector2.zero)
    //    {
    //        lookingAtBackground = false;
    //        GetComponent<Light>().range = baseLightRange;
    //        GetComponent<Light>().spotAngle = baseLightAngle;
    //        GetComponent<Light>().intensity = baseLightIntensity;
    //        transform.localScale = Vector3.one;
    //        LampTurnedOff.Invoke();
    //        gameObject.SetActive(false);
    //        isEnabled = false;
    //    }
    //}

    void CheckForMaxRotation()
    {
        if (transform.rotation.x > maxRotation.x)
            transform.rotation = Quaternion.Euler(maxRotation.x, transform.rotation.y, transform.rotation.z);
        if (transform.rotation.x < -maxRotation.x)
            transform.rotation = Quaternion.Euler(-maxRotation.x, transform.rotation.y, transform.rotation.z);

        if (transform.rotation.y > maxRotation.y)
            transform.rotation = Quaternion.Euler(transform.rotation.x, maxRotation.y,transform.rotation.z);
        if (transform.rotation.y < -maxRotation.y)
            transform.rotation = Quaternion.Euler(transform.rotation.x, -maxRotation.y, transform.rotation.z);
    }
}
