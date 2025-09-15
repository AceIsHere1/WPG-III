using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private CharacterController controller;
    [SerializeField] bool lockCursor = true;

    [Header("Footstep Settings")]
    public AudioClip walkSound;
    public AudioClip runSound;
    public float stepInterval = 0.5f;       // waktu antar langkah saat jalan
    public float runStepInterval = 0.3f;    // waktu antar langkah saat lari

    private AudioSource audioSource;
    private float stepTimer;

    float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;   // biar ga auto bunyi

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

        // 🎵 Cek footstep
        if (controller.isGrounded && velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = Input.GetKey(KeyCode.LeftShift) ? runStepInterval : stepInterval;
            }
        }
    }

    void PlayFootstep()
    {
        AudioClip clip = Input.GetKey(KeyCode.LeftShift) ? runSound : walkSound;

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log("🔊 Footstep played: " + clip.name);
        }
        else
        {
            Debug.LogWarning("⚠️ Footstep sound belum di-assign di Inspector!");
        }
    }
}
