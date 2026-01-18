using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Variables for player movemnt
    public Camera playerCamera;
    [SerializeField] public float walkSpeed;
    private float initialWalkSpeed;
    [SerializeField] public float runSpeed;
    private float initialRunSpeed;
    [SerializeField] public float jumpPower;
    [SerializeField] public float lookSpeed;
    [SerializeField] public float lookXLimit;
    [SerializeField] public float defaultHeight;
    [SerializeField] public float crouchHeight;
    [SerializeField] public float crouchSpeed;

    private float rotationX = 0;
    private CharacterController characterController;
    private Vector3 characterVelocity;

    private bool canMove = true;

    // Variables needed for hook shoot
    private float characterVelocityY;
    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;
    [SerializeField] private Transform hookshotTransform;
    private Vector3 characterVelocityMomentum;
    private CameraFov cameraFov;
    private ParticleSystem speedLinesParticleSystem;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    // Variables for dashing
    [Header("Stamina Bar")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaDrainRate;
    [SerializeField] private float staminaRegenRate;
    private float currentStamina;
    private bool isDashing = false;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;

    //Reference to the slider
    [SerializeField] private Slider staminaBar;

    private void Awake()
    {
        initialWalkSpeed = walkSpeed;
        initialRunSpeed = runSpeed;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<CameraFov>();
        speedLinesParticleSystem = transform.Find("Main Camera").Find("SpeedLinesParticleSystem").GetComponent<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);

        //Initialize StaminaBar to Max
        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
    }

    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                LookAround();
                Movement();
                Crouch();
                HandleHookshotStart();
                TryToDash();
                HandleStamina();
                break;
            case State.HookshootThrown:
                HandleHookshotThrow();
                LookAround();
                Movement();
                Crouch();
                HandleStamina();
                break;
            case State.HookshootFlyingPlayer:
                HandleHookshotMovement();
                LookAround();
                break;
        }
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        characterVelocity = transform.right * moveX * walkSpeed + transform.forward * moveZ * walkSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;
            // Jump
            if (TestInputJump())
            {
                characterVelocityY = jumpPower;
            }
        }

        // Apply gravity to the velocity
        float gravityDownForce = -40f;
        characterVelocityY += gravityDownForce * Time.deltaTime;


        // Apply Y velocity to move vector
        characterVelocity.y = characterVelocityY;

        // Apply momentum
        characterVelocity += characterVelocityMomentum;

        // Move Character Controller
        characterController.Move(characterVelocity * Time.deltaTime);

        // Dampen momentum
        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = initialWalkSpeed;
            runSpeed = initialRunSpeed;
        }
    }

    private void LookAround()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void TryToDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (currentStamina > 48)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(dashSpeed * Time.deltaTime * characterVelocity);
            yield return null;
        }
        isDashing = false;
    }

    /// <summary>
    /// Handle Stamina Bar
    /// </summary>
    private void HandleStamina()
    {
        //Using Stamina
        if (isDashing && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;

            //if totally used
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isDashing = false;
            }
        }
        //Regenerate Stamins
        else if (!isDashing && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }

        //Update Stamina bar
        staminaBar.value = currentStamina;
    }

    private void HandleHookshotStart()
    {
        if (TestInputDownHookshot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                // Hit something
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshootThrown;
            }
        }
    }

    private void HandleHookshotThrow()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 100f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshootFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
            speedLinesParticleSystem.Play();
        }
    }

    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 20f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;

        // Move Character Controller
        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            // Reached Hookshot Position
            StopHookshot();
        }

        if (TestInputDownHookshot())
        {
            // Cancel Hookshot
            StopHookshot();
        }

        if (TestInputJump())
        {
            // Cancelled with Jump
            float momentumExtraSpeed = 4f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            characterVelocityMomentum += Vector3.up * jumpPower;
            StopHookshot();
        }
    }
    private void StopHookshot()
    {
        state = State.Normal;
        //ResetGravityEffect();
        hookshotTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
        speedLinesParticleSystem.Stop();
    }


    private bool TestInputDownHookshot()
    {
        return Mouse.current.rightButton.isPressed;
    }

    private bool TestInputJump()
    {
        return Input.GetButton("Jump");
    }
}