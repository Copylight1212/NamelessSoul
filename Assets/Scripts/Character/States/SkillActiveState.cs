using UnityEngine;

public class SkillActiveState : ICharacterState
{
    private CharacterMovement character;
    private float skillElapsed;
    private bool delayedShown;

    public SkillActiveState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        skillElapsed = 0f;
        delayedShown = false;
    }

    public void Update()
    {
        // Drain mana
        float cost = character.Resources.skillManaPerSecondCost * Time.deltaTime;
        if (!character.Resources.CanUseMana(cost))
        {
            ExitSkill();
            return;
        }
        
        character.Resources.UseMana(cost);
        character.Resources.RecordManaUse();
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        // Delayed cape activation
        skillElapsed += Time.deltaTime;
        if (!delayedShown && skillElapsed >= character.Resources.delayedAppearTime)
        {
            delayedShown = true;
            if (character.Resources.skillCapeObject != null)
                character.Resources.skillCapeObject.SetActive(true);
        }
        
        // Check for skill toggle off
        if (character.IsSkillPressed)
        {
            character.ResetSkillInput();
            ExitSkill();
            return;
        }
        
        // Apply gravity
        character.Resources.ApplyGravity(Time.deltaTime);
        
        // Movement while skill is active
        bool isRunning = character.IsRunningInput && character.Resources.CanUseStamina(0.01f);
        float speed = isRunning ? character.Resources.runSpeed : character.Resources.walkSpeed;
        
        if (isRunning)
        {
            character.Resources.UseStamina(character.Resources.staminaDrainRate * Time.deltaTime);
        }
        else
        {
            character.Resources.RegenStamina(character.Resources.staminaRegenRate * Time.deltaTime);
        }
        
        Vector3 cameraForward = character.CameraTransform.forward;
        Vector3 cameraRight = character.CameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 horizontalMove = (cameraForward * character.InputDirection.z + cameraRight * character.InputDirection.x).normalized * speed;
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
        
        // Update animator
        character.Animator.SetBool("isMoving", character.InputDirection.magnitude > 0.01f);
        character.Animator.SetFloat("Speed", character.InputDirection.magnitude > 0.01f ? (isRunning ? 2f : 1f) : 0f);
    }

    private void ExitSkill()
    {
        if (character.Resources.skillVeilObject != null)
            character.Resources.skillVeilObject.SetActive(false);
        if (character.Resources.skillCapeObject != null)
            character.Resources.skillCapeObject.SetActive(false);
            
        if (character.InputDirection.magnitude > 0.01f)
        {
            character.TransitionToState(character.WalkingState);
        }
        else
        {
            character.TransitionToState(character.IdleState);
        }
    }

    public void Exit() { }
}