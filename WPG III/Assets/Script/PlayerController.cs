using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //camera movement
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;

    //character movement
    [SerializeField] private int speed = 5;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private CharacterController controller;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (lockCursor) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -45.0f, 45.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Ambil arah kamera
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        // Buat flat (hilangkan komponen Y biar tidak miring)
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Hitung arah gerakan berdasarkan kamera
        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // Kalau ada input (bukan idle)
        if (moveDirection.magnitude > 0.1f)
        {
            // Rotasi player ke arah gerakan
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDirection),
                Time.deltaTime * 10f
            );
        }

        // Gerakkan player
        controller.Move(moveDirection * speed * Time.deltaTime);

        //moveDirection = new Vector3(horizontal, 0, vertical);

        //moveDirection *= speed;

        //if ((moveDirection.x != 0) || (moveDirection.z != 0))
        //{
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime);
        //}

        //controller.Move(moveDirection * Time.deltaTime);

        //Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //inputDir.Normalize();

        //Vector3 velocity = (transform.forward * inputDir.y + transform.right * inputDir.x) * speed;

        //controller.Move(velocity * Time.deltaTime);

        if (moveDirection == Vector3.zero)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 0.5f);
        }
    }
}
