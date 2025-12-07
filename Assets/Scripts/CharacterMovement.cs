using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInputObserver
{
    [Header("Movement Settings")]
    public float walkSpeed = 100f;
    public float runSpeed = 200f;
    
    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;
    
    private CharacterController controller;
    private InputManager inputManager;
    private Vector3 inputDirection;
    private bool isRunning;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Register this component as an observer
        if (inputManager != null)
        {
            inputManager.RegisterObserver(this);
        }
        else
        {
            Debug.LogError("InputManager not found on the same GameObject!");
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
        isRunning = input.isRunning;
    }

    void Update()
    {
        HandleMovement();
        HandleAnimator();
    }

    void HandleMovement()
    {
        float speed = isRunning ? runSpeed : walkSpeed;

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
        
        float targetSpeed = 0f;

        if (isCurrentlyMoving)
        {
            targetSpeed = isRunning ? 2f : 1f;
        }

        animator.SetFloat("Speed", targetSpeed);
    }
}