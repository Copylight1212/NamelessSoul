using UnityEngine;

public class MovementInput
{
    public Vector3 direction;
    public bool isRunning;
    public float zoom;
    
    public bool isJumpPressed; // Detects button down event
    public bool isSkillPressed;
    public bool isDashPressed;
}

public interface IInputObserver
{
    void OnInputChanged(MovementInput input);
}