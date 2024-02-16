using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public LayerMask whatIsGround;
    public float groundClearance = 1.0f;

    private float xRotation = 0.0f;
    private Transform cameraTransform;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = transform.GetChild(0);
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // Forward and backward movement
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += cameraTransform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= cameraTransform.forward;
        }

        // Left and right movement
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= cameraTransform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += cameraTransform.right;
        }
        
        moveDirection.Normalize();
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        
        if (IsCloseToGround(newPosition))
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized;
            newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        }

        if (!IsBelowGround(newPosition))
        {
            transform.position = newPosition;
        }

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    bool IsCloseToGround(Vector3 position)
    {
        RaycastHit hit;
        return Physics.Raycast(position, Vector3.down, out hit, groundClearance, whatIsGround);
    }

    bool IsBelowGround(Vector3 position)
    {
        RaycastHit hit;
        return Physics.Raycast(position, Vector3.down, out hit, 0.1f, whatIsGround);
    }
}
