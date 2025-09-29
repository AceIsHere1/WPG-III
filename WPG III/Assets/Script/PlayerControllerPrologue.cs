using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPrologue : MonoBehaviour
{
    
    [Header("Camera Settings")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private CharacterController controller;
    [SerializeField] bool lockCursor = true;

    [Header("Audio Settings")]
    public AudioSource footstepsSound; // langkah kaki
    public AudioSource sprintSound;    // lari

    float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // pastikan audio tidak langsung nyala
        footstepsSound.Stop();
        sprintSound.Stop();
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -60.0f, 60.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir.Normalize();

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 velocity = (transform.forward * inputDir.y + transform.right * inputDir.x) * currentSpeed;

        controller.Move(velocity * Time.deltaTime);

        bool isMoving = inputDir.magnitude > 0.1f;

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift)) // lari
            {
                if (!sprintSound.isPlaying)
                {
                    footstepsSound.Stop();
                    sprintSound.Play();
                }
            }
            else // jalan
            {
                if (!footstepsSound.isPlaying)
                {
                    sprintSound.Stop();
                    footstepsSound.Play();
                }
            }
        }
        else
        {
            // berhenti -> matikan semua suara
            footstepsSound.Stop();
            sprintSound.Stop();
        }
    }

}
