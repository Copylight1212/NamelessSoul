using UnityEngine;

public class SkillCastingState : ICharacterState
{
    private CharacterMovement character;
    private float castTimer;

    public SkillCastingState(CharacterMovement character)
    {
        this.character = character;
    }

    public void Enter()
    {
        character.Resources.UseMana(character.Resources.skillInitialManaCost);
        character.Resources.RecordManaUse();
        
        castTimer = 0f;
        
        if (character.Resources.skillVeilObject != null)
            character.Resources.skillVeilObject.SetActive(true);
            
        character.Animator.SetTrigger(character.Resources.skillTriggerName);
        character.Animator.SetFloat("Speed", 0f);
    }

    public void Update()
    {
        character.Resources.HandleHealthRegen(Time.deltaTime);
        
        // Lock in place
        character.Controller.Move(Vector3.zero);
        
        castTimer += Time.deltaTime;
        
        if (castTimer >= character.Resources.skillCastTime)
        {
            character.TransitionToState(character.SkillActiveState);
        }
    }

    public void Exit() { }
}