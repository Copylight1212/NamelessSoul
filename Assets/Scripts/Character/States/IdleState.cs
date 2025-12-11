using UnityEngine;

public class IdleState : ICharacterState
{
    private CharacterMovement character;

    public IdleState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        character.Animator.SetBool("isMoving", false);
        character.Animator.SetFloat("Speed", 0f);
    }

    public void Update()
    {
        // Handle resource regeneration
        character.Resources.RegenStamina(character.Resources.staminaRegenRate * Time.deltaTime);
        character.Resources.HandleManaRegen(Time.deltaTime);
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        // Apply gravity
        character.Resources.ApplyGravity(Time.deltaTime);
        character.Controller.Move(new Vector3(0, character.Resources.VerticalVelocity, 0) * Time.deltaTime);
        
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
        
        if (character.InputDirection.magnitude > 0.01f)
        {
            character.TransitionToState(character.WalkingState);
        }
    }

    public void Exit() { }
}