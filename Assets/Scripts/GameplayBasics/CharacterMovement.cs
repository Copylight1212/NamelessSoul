using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour, IInputObserver
{
    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;
    
    private CharacterController controller;
    private InputManager inputManager;
    private PlayerResources playerResources;
    
    // Current state
    private ICharacterState currentState;
    
    // All states
    private IdleState idleState;
    private WalkingState walkingState;
    private SprintingState sprintingState;
    private JumpingState jumpingState;
    private DashingState dashingState;
    private SkillCastingState skillCastingState;
    private SkillActiveState skillActiveState;
    
    // Input data
    private Vector3 inputDirection;
    private bool isRunningInput;
    private bool isJumpPressed;
    private bool isSkillPressed;
    private bool isDashPressed;
    
    private Vector3 lastMeshLocalPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        playerResources = GetComponent<PlayerResources>();
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (inputManager != null)
        {
            inputManager.RegisterObserver(this);
        }
        
        if (animator != null)
        {
            lastMeshLocalPosition = animator.transform.localPosition;
        }
        
        // Initialize all states
        idleState = new IdleState(this);
        walkingState = new WalkingState(this);
        sprintingState = new SprintingState(this);
        jumpingState = new JumpingState(this);
        dashingState = new DashingState(this);
        skillCastingState = new SkillCastingState(this);
        skillActiveState = new SkillActiveState(this);
        
        // Start in idle state
        TransitionToState(idleState);
    }

    void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.UnregisterObserver(this);
        }
    }

    public void OnInputChanged(MovementInput input)
    {
        inputDirection = input.direction;
        isRunningInput = input.isRunning;
        isJumpPressed = input.isJumpPressed;
        isSkillPressed = input.isSkillPressed;
        isDashPressed = input.isDashPressed;
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void TransitionToState(ICharacterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // Getters for states to access
    public CharacterController Controller => controller;
    public PlayerResources Resources => playerResources;
    public Animator Animator => animator;
    public Transform CameraTransform => cameraTransform;
    public Vector3 InputDirection => inputDirection;
    public bool IsRunningInput => isRunningInput;
    public bool IsJumpPressed => isJumpPressed;
    public bool IsSkillPressed => isSkillPressed;
    public bool IsDashPressed => isDashPressed;
    
    // State getters
    public IdleState IdleState => idleState;
    public WalkingState WalkingState => walkingState;
    public SprintingState SprintingState => sprintingState;
    public JumpingState JumpingState => jumpingState;
    public DashingState DashingState => dashingState;
    public SkillCastingState SkillCastingState => skillCastingState;
    public SkillActiveState SkillActiveState => skillActiveState;
    
    public void ResetJumpInput() => isJumpPressed = false;
    public void ResetDashInput() => isDashPressed = false;
    public void ResetSkillInput() => isSkillPressed = false;
    
    public Vector3 GetAnimationMovementDelta()
    {
        if (animator != null)
        {
            Vector3 meshLocalDelta = animator.transform.localPosition - lastMeshLocalPosition;
            animator.transform.localPosition = lastMeshLocalPosition;
            return meshLocalDelta;
        }
        return Vector3.zero;
    }
}