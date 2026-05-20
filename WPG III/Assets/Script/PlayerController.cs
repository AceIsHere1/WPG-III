using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float minCameraPitch = -70.0f; // How far down you can look
    [SerializeField] float maxCameraPitch = 80.0f;  // How far up you can look

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private CharacterController controller;
    [SerializeField] bool lockCursor = true;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -15f; // Pulls the player down
    private float velocityY = 0f; // Stores current downward speed

    public AudioSource footstepsSound, sprintSound;

    float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        // Jika game sedang pause, hentikan kontrol player
        if (PauseManager.isGamePaused)
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
            return;
        }

        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        // 1. Reset gravity if standing on the ground so it doesn't build up infinitely
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -5f; // A constant downward snap to stick to sloped bridges
        }

        // 2. Calculate horizontal movement (X and Z axes)
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir.Normalize();

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 moveDir = (transform.forward * inputDir.y + transform.right * inputDir.x) * currentSpeed;

        // 3. Apply gravity to the vertical movement (Y axis)
        velocityY += gravity * Time.deltaTime;
        
        // 4. Combine horizontal movement and vertical gravity into one final velocity
        Vector3 finalVelocity = moveDir + (Vector3.up * velocityY);

        // 5. Move the controller
        controller.Move(finalVelocity * Time.deltaTime);

        // Sound logic
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                footstepsSound.enabled = false;
                sprintSound.enabled = true;
            }
            else
            {
                footstepsSound.enabled = true;
                sprintSound.enabled = false;
            }
        }
        else
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
        }
    }
}