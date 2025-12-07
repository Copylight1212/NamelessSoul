using UnityEngine;

public class MovementInput
{
    public Vector3 direction;
    public bool isRunning;
    public float zoom;
}

public interface IInputObserver
{
    void OnInputChanged(MovementInput input);
}