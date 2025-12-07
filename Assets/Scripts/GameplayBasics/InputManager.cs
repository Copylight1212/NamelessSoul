using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    [Header("Movement Key Bindings")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Camera Key Bindings")]
    public KeyCode zoomInKey = KeyCode.KeypadPlus;      // + key (same key as =)
    public KeyCode zoomOutKey = KeyCode.KeypadMinus;      // - key
    public float keyZoomSpeed = 0.1f;               // How much to zoom per frame when holding key

    private MovementInput currentInput;
    private List<IInputObserver> observers = new List<IInputObserver>();

    void Awake()
    {
        if (currentInput == null) 
        {
            currentInput = new MovementInput();
        }
    }

    public void RegisterObserver(IInputObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
            Debug.Log($"Observer registered. Total observers: {observers.Count}");
        }
    }

    public void UnregisterObserver(IInputObserver observer)
    {
        observers.Remove(observer);
    }

    void Update()
    {
        // Gather movement input
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(upKey)) v = 1;
        if (Input.GetKey(downKey)) v = -1;
        if (Input.GetKey(leftKey)) h = -1;
        if (Input.GetKey(rightKey)) h = 1;

        currentInput.direction = new Vector3(h, 0f, v);
        currentInput.isRunning = Input.GetKey(runKey);

        // Gather zoom input (scroll wheel + keys)
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        
        if (Input.GetKey(zoomInKey))
        {
            zoom += keyZoomSpeed;
        }
        if (Input.GetKey(zoomOutKey))
        {
            zoom -= keyZoomSpeed;
        }

        currentInput.zoom = zoom;

        // Notify all observers
        NotifyObservers();
    }

    void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer?.OnInputChanged(currentInput);
        }
    }
}