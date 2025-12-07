using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour, IInputObserver
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;

    [Header("Rotation")]
    public float sensitivityX = 180f;
    public float sensitivityY = 120f;
    public float minYAngle = -30f;
    public float maxYAngle = 70f;

    private float yaw;
    private float pitch;
    private InputManager inputManager;
    private float zoomInput;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find and register with InputManager
        inputManager = FindFirstObjectByType<InputManager>();
        if (inputManager != null)
        {
            inputManager.RegisterObserver(this);
        }
    }

    void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.UnregisterObserver(this);
        }
    }

    // Called by InputManager
    public void OnInputChanged(MovementInput input)
    {
        zoomInput = input.zoom;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Zoom (using zoom from InputManager - works with scroll wheel AND +/- keys)
        distance -= zoomInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Mouse rotation (direct input, not from InputManager)
        yaw += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);

        // Apply rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Position behind target
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.rotation = rotation;
    }
}