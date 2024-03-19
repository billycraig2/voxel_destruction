using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {

    float mainSpeed = 50.0f; // Regular speed
    float shiftAdd = 250.0f; // Multiplied by how long shift is held. Basically running
    float maxShift = 100.0f; // Maximum speed when holding shift
    float camSens = 1f; // How sensitive it with mouse

    private float totalRun = 1.0f;

    void Start() {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        // Calculate new rotation
        float mouseX = Input.GetAxis("Mouse X") * camSens;
        float mouseY = Input.GetAxis("Mouse Y") * camSens;

        Vector3 lookhere = new Vector3(-mouseY, mouseX, 0);
        transform.eulerAngles = transform.eulerAngles + lookhere;

        // Keyboard commands
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0) { // Only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift)) {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            } else {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space)) { // If player wants to move on X and Z axis only
                transform.Translate(p);
                Vector3 newPosition = transform.position;
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            } else {
                transform.Translate(p);
            }
        }

        // Toggle cursor lock state with escape key
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = (Cursor.lockState != CursorLockMode.Locked);
        }
    }

    private Vector3 GetBaseInput() { // Returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S)) {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A)) {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
