using UnityEngine;

public class WalkingState : ICharacterState
{
    private CharacterMovement character;

    public WalkingState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        character.Animator.SetBool("isMoving", true);
        character.Animator.SetFloat("Speed", 1f);
    }

    public void Update()
    {
        // Handle resource regeneration
        character.Resources.RegenStamina(character.Resources.staminaRegenRate * Time.deltaTime);
        character.Resources.HandleManaRegen(Time.deltaTime);
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        // Apply gravity
        character.Resources.ApplyGravity(Time.deltaTime);
        
        // Movement
        Vector3 cameraForward = character.CameraTransform.forward;
        Vector3 cameraRight = character.CameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 horizontalMove = (cameraForward * character.InputDirection.z + cameraRight * character.InputDirection.x).normalized * character.Resources.walkSpeed;
        Vector3 animDelta = character.GetAnimationMovementDelta();
        
        Vector3 finalMove = horizontalMove;
        finalMove.y = character.Resources.VerticalVelocity + (animDelta.y / Time.deltaTime);
        
        character.Controller.Move(finalMove * Time.deltaTime);

        // Rotation
        if (character.InputDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMove.normalized);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        // Check transitions
        if (character.IsSkillPressed)
        {
            character.ResetSkillInput();
            if (character.Resources.CanUseMana(character.Resources.skillInitialManaCost))
            {
                character.TransitionToState(character.SkillCastingState);
                return;
            }
        }
        
        if (character.IsDashPressed && character.Controller.isGrounded)
        {
            character.ResetDashInput();
            if (character.Resources.CanUseStamina(character.Resources.dashDrainCost))
            {
                character.TransitionToState(character.DashingState);
                return;
            }
        }
        
        if (character.IsJumpPressed && character.Controller.isGrounded)
        {
            character.ResetJumpInput();
            if (character.Resources.CanUseStamina(character.Resources.jumpDrainCost))
            {
                character.TransitionToState(character.JumpingState);
                return;
            }
        }
        
        if (character.IsRunningInput && character.Resources.CanUseStamina(0.01f))
        {
            character.TransitionToState(character.SprintingState);
            return;
        }
        
        if (character.InputDirection.magnitude < 0.01f)
        {
            character.TransitionToState(character.IdleState);
        }
    }

    public void Exit() { }
}