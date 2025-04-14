using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerInput plrInputs;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 modificator;
    private float rotation;
    private float rotationSpeed = 5.0f;
    private float interpolation;

    float actualSpeed = 5.0f;
    public float walkSpeed = 5.0f;
    public float crouchSpeed = 5.0f;
    public float sprintSpeed = 25.0f;
    public float aimAtBackGroundSpeedMultiplier = 0.75f;
    public float jumpPower = 20.0f;
    public float jumpSpeedDivider = 2.0f;
    public float gravityScale = 5f;
    public float climbSpeed = 2.0f;
    public bool immovable = false;
    private Vector3 offset = new(0, 0, 0);


    public bool crouching = false;
    public bool climbing = false;
    private bool sprinting = false;
    private bool jumpHold = false;
    private float jumpCD = 0.8f;
    private float jumpTimer = 0;
    private List<GameObject> Interactions;
    private GameObject actualInteraction;
    bool jump = false;

    public LayerMask ignore;

    public bool onGround;

    PlayerManager playerManager;
    Animator playerAnimator;
    enum PlayerState
    {
        Walking,
        Running,
        Jumping,
        Crouching,
        Immobile
    }

    PlayerState state;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        plrInputs = GetComponent<PlayerInput>();
        Interactions = new List<GameObject>();
        playerManager = GameManager.Get.playerManager;
        playerAnimator = GetComponentInChildren<Animator>();
        //InputManager.Get.Move.AddListener(InputMove);
        //InputManager.Get.Jump.AddListener(InputJump);
        //InputManager.Get.Crouch.AddListener(InputCrouch);

        StartCoroutine(FootstepSoundCoroutine());

    }

    //Inputs
    public void InputMove()
    {
        if (plrInputs.actions["Move"].ReadValue<Vector2>().x > 0.1f)
        {
            direction.x = actualSpeed + offset.x;
            playerAnimator.SetBool("isFacingLeft", false);
            playerAnimator.SetBool("isFacingRight", true);
            playerAnimator.SetBool("isWalking", true);
            direction.x = actualSpeed;
        }
        else
        {
            if (plrInputs.actions["Move"].ReadValue<Vector2>().x < -0.1f)
            {
                direction.x = -actualSpeed + offset.x;
                playerAnimator.SetBool("isFacingLeft", true);
                playerAnimator.SetBool("isFacingRight", false);
                playerAnimator.SetBool("isWalking", true);
                direction.x = -actualSpeed;
            }
            else
            {
                if (!climbing)
                {
                    direction.x = 0 + offset.x;
                    playerAnimator.SetBool("isWalking", false);
                    direction.x = 0;
                }
            }

        }
        if (!controller.isGrounded)
        {
            direction.x = direction.x / jumpSpeedDivider + offset.x;
        }

    }

    void InputClimb()
    {
        if (climbing)
        {
            direction.x = 0;
            if (Mathf.Abs(plrInputs.actions["Move"].ReadValue<Vector2>().y) > 0.2f)
            {
                direction.y += plrInputs.actions["Move"].ReadValue<Vector2>().y * climbSpeed * Time.deltaTime;
            }
            else
            {
                direction.y = 0;
            }

        }
    }

    void SpeedCheck()
    {

        if (Mathf.Abs(plrInputs.actions["Move"].ReadValue<Vector2>().x) > 0.9f)
        {
            if (!crouching)
            {
                state = PlayerState.Running;
                actualSpeed = sprintSpeed;
            }
            else
            {
                actualSpeed = crouchSpeed;
            }


        }
        if (Mathf.Abs(plrInputs.actions["Move"].ReadValue<Vector2>().x) < 0.9f && Mathf.Abs(plrInputs.actions["Move"].ReadValue<Vector2>().x) > 0)
        {
            if (!crouching)
            {
                state = PlayerState.Walking;
                actualSpeed = walkSpeed;
            }
            else
            {
                actualSpeed = crouchSpeed;
            }
        }
        if (playerManager.lookingAtBackground)
        {
            actualSpeed *= aimAtBackGroundSpeedMultiplier;
        }
        if (plrInputs.actions["Move"].ReadValue<Vector2>().x == 0)
        {
            actualSpeed = 0;
            state = PlayerState.Immobile;
        }
    }

    public void InputJump(InputAction.CallbackContext cbx)
    {

        if (climbing)
        {
            AudioManager.Get.Play("jump_hero_concrete", transform);
            climbing = false;
            direction.y += jumpPower;
        }

        if (controller.isGrounded && plrInputs.actions["Jump"].ReadValue<float>() == 1 && cbx.started && climbing == false && jump == false)
        {
            AudioManager.Get.Play("jump_hero_concrete", transform);
            state = PlayerState.Jumping;
            jump = true;
            jumpTimer = Time.realtimeSinceStartup + jumpCD;
            direction.y += jumpPower;
        }

        if (plrInputs.actions["Jump"].ReadValue<float>() == 0)
        {
            jumpHold = false;
        }
        else
        {
            jumpHold = true;
        }
    }

    private GameObject NearestInteraction()
    {
        if (Interactions.Count > 0)
        {
            actualInteraction = Interactions[0];
            foreach (GameObject obj in Interactions)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, actualInteraction.transform.position))
                {
                    actualInteraction = obj;
                }
            }
            return actualInteraction;
        }
        return null;

    }

    public void InputInteract(InputAction.CallbackContext cbx)
    {
        if ((cbx.started || plrInputs.actions["Interaction"].ReadValue<float>() == 0) && Interactions.Count > 0)
        {
            if (plrInputs.actions["Interaction"].ReadValue<float>() > 0)
            {
                NearestInteraction().GetComponent<Interaction>().Interact(false);
                actualInteraction = NearestInteraction();
            }
            else
            {
                if (actualInteraction)
                    actualInteraction.GetComponent<Interaction>().Interact(true);

            }
        }
    }

    public void InputCrouch(InputAction.CallbackContext cbx)
    {
        if (!sprinting && controller.isGrounded && cbx.started && !GameManager.Get.playerManager.isCarrying && climbing == false)
        {
            if (plrInputs.actions["Crouch"].ReadValue<float>() > 0)
            {
                if (crouching)
                {
                    if (!Physics.Raycast(transform.position, Vector3.up, 1))
                    {
                        state = PlayerState.Crouching;
                        crouching = false;
                        playerAnimator.SetBool("isCrouching", false);
                        controller.height = 2f;
                        controller.center = new Vector3(controller.center.x, 0, controller.center.z);
                        actualSpeed = walkSpeed;
                        InputMove();
                    }

                }
                else
                {
                    state = PlayerState.Immobile;
                    crouching = true;
                    playerAnimator.SetBool("isCrouching", true);
                    controller.height = 0.5f;
                    controller.center = new Vector3(controller.center.x, -0.5f, controller.center.z);
                    actualSpeed = crouchSpeed;
                    InputMove();

                }
            }
        }
    }


    //Movement
    private void PlayerLook()
    {
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, rotation, gameObject.transform.rotation.z);
        interpolation -= (plrInputs.actions["Move"].ReadValue<Vector2>().x) * rotationSpeed * Time.deltaTime;
        interpolation = Mathf.Clamp(interpolation, 0, 1);
        rotation = Mathf.Lerp(0, 180, interpolation);
    }

    private void ApplyGravity()
    {
        if (climbing == false)
        {
            if (!controller.isGrounded)
            {
                direction.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            }
            else
            {
                direction.y = -1;
            }
        }

        if (offset.x > 0)
        {
            offset.x -= 9.81f * Time.deltaTime;
        }
        else if (offset.x < 0)
        {
            offset.x += 9.81f * Time.deltaTime;
        }


    }

    void JumpHold()
    {
        if (jumpHold && Time.realtimeSinceStartup < jumpTimer)
        {
            direction.y += 30f * Time.deltaTime;
        }

        if (jumpTimer - 0.6f < Time.realtimeSinceStartup)
        {
            jump = false;
        }
    }

    public void AddForce(Vector3 force)
    {
        offset = force;
        direction.y = 0;
        direction.x += force.x;
        direction.y += force.y;
    }

    void RoofCheck()
    {
        if (!controller.isGrounded && Time.realtimeSinceStartup < jumpTimer - .2f)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1.1f, ~ignore) && !hit.transform.gameObject.CompareTag("light"))
            {
                jumpHold = false;
                direction.y -= 1;
            }
        }
    }



    void Update()
    {
        if (GameManager.Get.isPlaying)
            if (!immovable)
            {
                JumpHold();
                RoofCheck();
                SpeedCheck();
                OnGround();
                if (onGround)
                    CheckPosition();
                InputMove();
                InputClimb();
                controller.Move((direction + modificator) * Time.deltaTime);
                PlayerLook();
                ApplyGravity();
            }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact") && !Interactions.Contains(other.gameObject))
        {
            Interactions.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            Interactions.Remove(other.gameObject);
        }
    }
    public void CheckPosition()
    {
        if (modificator != Vector3.zero)
            modificator = Vector3.zero;
        if (transform.position.z != 0)
        {
            controller.enabled = false;
            Vector3 t = transform.position;
            t.z = 0f;
            transform.position = t;
            controller.enabled = true;
        }
    }

    public void OnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                if (onGround == false)
                    onGround = true;
            }
            else
            {
                if (onGround == true)
                    onGround = false;
            }
        }
    }

    IEnumerator FootstepSoundCoroutine()
    {
        while (true)
        {
            if (state == PlayerState.Running && controller.isGrounded)
            {
                yield return new WaitForSeconds(0.4f);
                AudioManager.Get.Play("footstep_hero_concreate_4");
            }
            else if (state == PlayerState.Walking && controller.isGrounded)
            {
                yield return new WaitForSeconds(0.6f);
                AudioManager.Get.Play("footstep_hero_concreate_4", transform);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
