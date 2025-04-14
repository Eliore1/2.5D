using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

public class PlayerCamera : MonoBehaviour
{
    #region Settings
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;
    [SerializeField] private float returnSmoothTime;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float maxDistance;
    [SerializeField] private float value;
    [SerializeField] private float maxDistanceCameraPlayer;
    private Vector3 velocity = Vector3.zero;
    private Vector3 originOffset;
    private Vector3 shakeOffset;

    #endregion


    #region TargetLook
    [HideInInspector] public Vector2 look = Vector2.zero;
    Vector3 target = Vector3.zero;
    [SerializeField] private float lookValue;
    private bool collide = false;

    PlayerManager playerManager;
    #endregion

    void Awake()
    {

        originOffset = offset;
    }
    private void Start()
    {
        playerManager = GameManager.Get.playerManager;
    }

    void Update()
    {
        CollisionCameraObject();
        ActualizeOffset();
        target = player.position + offset;
        if (playerManager.lookingAtBackground)
            target = playerManager.transform.position+ originOffset+ Vector3.up*2;
        if (!collide)
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        else if (collide && new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, 0f).magnitude >= maxDistanceCameraPlayer)
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

    }
    // 

    public void LookPlayer()
    {
        if (offset.x == 0)
            offset.x = value;
        else if (player.rotation.eulerAngles.y > 160 && offset.x > 0)
            offset.x *= -1;
        else if (player.rotation.eulerAngles.y < 20 && offset.x < 0)
            offset.x *= -1;
    }

    public void LookPlayerCenter()
    {
        offset = originOffset;
    }
    public void ActualizeOffset()
    {
        if (collide)
            LookPlayerCenter();
        else if (look == Vector2.zero && GameManager.Get.playerManager.playerMovement.plrInputs.actions["Move"].ReadValue<Vector2>() == Vector2.zero)
            LookPlayerCenter();
        else if (look == Vector2.zero && GameManager.Get.playerManager.playerMovement.plrInputs.actions["Move"].ReadValue<Vector2>() != Vector2.zero && !collide)
            LookPlayer();
        else if (!GameManager.Get.playerManager.lookingAtBackground)
        {
            Vector2 directionLook = look;
            directionLook.Normalize();
            if (directionLook.x > 0)
                offset.x =  originOffset.x + lookValue;
            else if (directionLook.x < 0)
                offset.x = originOffset.x - lookValue;

            if (directionLook.y > 0)
                offset.y = originOffset.y + lookValue; 
        }

    }
    public void Zoom(Transform _target)
    {
        target = _target.position + offset;
    }

    public void LookTowards(InputAction.CallbackContext context)
    {
        Vector2 tLook = context.ReadValue<Vector2>();
        look = tLook;
    }

    IEnumerator ShakeOnce(float magnitude)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 amount = new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
            transform.position += amount;
            yield return new WaitForSeconds(.02f);
            transform.position -= amount;
        }
        
    }


    public void CamShake(float magnitude)
    {
        StartCoroutine(ShakeOnce(magnitude));
    }

    public void CollisionCameraObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Obstacle")))
        {
            if(hit.point.z < player.transform.position.z)
                collide = true;
            else
                collide = false;
        }
        else
            collide = false;    
    }
    
    
}
