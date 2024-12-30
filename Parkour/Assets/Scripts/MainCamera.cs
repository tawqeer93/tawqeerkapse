using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; 
    public float distanceFromTarget = 5f; 
    public float rotationSpeed = 3f; 

    [Header("Camera Constraints")]
    public float minVerticalAngle = -14f; 
    public float maxVerticalAngle = 45f; 
    public Vector2 framingBalance; 

    [Header("Invert Controls")]
    public bool invertX; 
    public bool invertY; 

    float currentRotationX; 
    float currentRotationY; 
    float invertedXValue;
    float invertedYValue;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        invertedXValue = (invertX) ? -1 : 1;
        invertedYValue = (invertY) ? -1 : 1;

        currentRotationX += Input.GetAxis("Mouse Y") * invertedYValue * rotationSpeed;
        currentRotationX = Mathf.Clamp(currentRotationX, minVerticalAngle, maxVerticalAngle);
        currentRotationY += Input.GetAxis("Mouse X") * invertedXValue * rotationSpeed;

        var targetRotation = Quaternion.Euler(currentRotationX, currentRotationY, 0); 

        var focusPosition = target.position + new Vector3(framingBalance.x, framingBalance.y); 

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distanceFromTarget); 
        transform.rotation = targetRotation; 
    }

    public Quaternion flatRotation => Quaternion.Euler(0, currentRotationY, 0);
}
