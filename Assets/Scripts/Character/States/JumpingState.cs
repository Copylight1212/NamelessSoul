using UnityEngine;

public class JumpingState : ICharacterState
{
    private CharacterMovement character;

    public JumpingState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        character.Resources.UseStamina(character.Resources.jumpDrainCost);
        character.Resources.SetVerticalVelocity(character.Resources.jumpForce);
        character.Animator.SetTrigger("Jump");
    }

    public void Update()
    {
        character.Resources.HandleManaRegen(Time.deltaTime);
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        // Apply gravity
        character.Resources.ApplyGravity(Time.deltaTime);
        
        // Air movement
        Vector3 cameraForward = character.CameraTransform.forward;
        Vector3 cameraRight = character.CameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 horizontalMove = (cameraForward * character.InputDirection.z + cameraRight * character.InputDirection.x).normalized * character.Resources.walkSpeed;
        
        Vector3 finalMove = horizontalMove;
        finalMove.y = character.Resources.VerticalVelocity;
        
        character.Controller.Move(finalMove * Time.deltaTime);

        // Rotation
        if (character.InputDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMove.normalized);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        // Check if landed
        if (character.Controller.isGrounded && character.Resources.VerticalVelocity < 0)
        {
            if (character.InputDirection.magnitude > 0.01f)
            {
                if (character.IsRunningInput && character.Resources.CanUseStamina(0.01f))
                {
                    character.TransitionToState(character.SprintingState);
                }
                else
                {
                    character.TransitionToState(character.WalkingState);
                }
            }
            else
            {
                character.TransitionToState(character.IdleState);
            }
        }
    }

    public void Exit() { }
}