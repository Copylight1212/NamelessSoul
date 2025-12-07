using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInputObserver
{
    [Header("Movement Settings")]
    public float walkSpeed = 100f;
    public float runSpeed = 200f;

    public float staminaDrainRate = 5f;
    public float staminaRegenRate = 5f;
    
    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;
    
    private CharacterController controller;
    
    private InputManager inputManager;
    private PlayerResources playerResources; // Reference to the PlayerStats script

    
    private Vector3 inputDirection;
    private bool isRunningInput;
    private bool canRun;         // Store if running is currently allowed (enough stamina)


    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        playerResources = GetComponent<PlayerResources>();
        
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Register this component as an observer
        if (inputManager != null)
        {
            inputManager.RegisterObserver(this);
        }
    }

    void OnDestroy()
    {
        // Unregister when destroyed
        if (inputManager != null)
        {
            inputManager.UnregisterObserver(this);
        }
    }

    // Called by InputManager when input changes
    public void OnInputChanged(MovementInput input)
    {
        inputDirection = input.direction;
        isRunningInput = input.isRunning;
    }

    void Update()
    {
        ManageStamina(); 

        
        HandleMovement();
        HandleAnimator();
    }

    private void ManageStamina()
    {
        bool isMoving = inputDirection.magnitude > 0.01f;
        
        if (isRunningInput && canRun && isMoving)
        {
            // Drain stamina if running is requested and allowed
            playerResources.UseStamina(staminaDrainRate * Time.deltaTime);
            
            // Check if stamina hit zero
            if (playerResources.StaminaPercentage() <= 0.01f) // Use percentage for safety
            {
                canRun = false; // Disable running until stamina recovers
            }
        }
        else if (isMoving || !isRunningInput) // If not running, or if stamina is depleted
        {
            // Regen stamina if idle or walking/sprinting is disabled
            playerResources.RegenStamina(staminaRegenRate * Time.deltaTime);
            
            // Re-enable running if stamina is above a certain threshold (e.g., 20%)
            if (!canRun && playerResources.StaminaPercentage() > 0.2f) 
            {
                canRun = true;
            }
            
        }
    }

    void HandleMovement()
    {
        bool actuallyRunning = isRunningInput && canRun;
        float speed = actuallyRunning ? runSpeed : walkSpeed;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
    
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;

        if (inputDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            controller.SimpleMove(moveDirection * speed);
        }
    }

    void HandleAnimator()
    {
        bool isCurrentlyMoving = inputDirection.magnitude > 0.01f;
        
        bool actuallyRunning = isRunningInput && canRun;
        
        float targetSpeed = 0f;

        if (isCurrentlyMoving)
        {
            targetSpeed = actuallyRunning ? 2f : 1f;
        }

        animator.SetFloat("Speed", targetSpeed);
    }
}