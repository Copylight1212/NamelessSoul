using UnityEngine;

public class DashingState : ICharacterState
{
    private CharacterMovement character;
    private float dashTimer;
    private Vector3 dashDirection;

    public DashingState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        character.Resources.UseStamina(character.Resources.dashDrainCost);
        
        // Get dash direction
        if (character.InputDirection.magnitude > 0.01f)
        {
            Vector3 camForward = character.CameraTransform.forward;
            Vector3 camRight = character.CameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            dashDirection = (camForward * character.InputDirection.z + camRight * character.InputDirection.x).normalized;
        }
        else
        {
            dashDirection = character.transform.forward;
        }

        character.transform.rotation = Quaternion.LookRotation(dashDirection);
        dashTimer = character.Resources.dashDuration;
        character.Animator.SetTrigger("Dash");
    }

    public void Update()
    {
        character.Resources.HandleManaRegen(Time.deltaTime);
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        dashTimer -= Time.deltaTime;
        
        if (dashTimer <= 0f)
        {
            // Dash complete
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
            return;
        }
        
        // Dash movement
        float dashSpeed = character.Resources.dashDistance / character.Resources.dashDuration;
        Vector3 dashVelocity = dashDirection * dashSpeed;
        dashVelocity.y = 0f;
        character.Controller.Move(dashVelocity * Time.deltaTime);
    }

    public void Exit() { }
}
