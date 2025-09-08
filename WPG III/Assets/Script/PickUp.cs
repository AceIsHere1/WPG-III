using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;      // Kamera player
    public float holdDistance = 2f;     // Jarak object dari kamera
    public float smoothSpeed = 10f;     // Kecepatan smoothing biar gak getar

    private bool pickedUp;
    private Rigidbody rb;

    [Header("Sound Settings")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (pickedUp)
        {
            // hitung posisi depan kamera (pas crosshair)
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;

            // gerakkan object smooth ke targetPos
            rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * smoothSpeed));

            // rotasi ikut kamera biar lebih natural
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));
        }
    }

    void OnMouseDown()
    {
        pickedUp = true;

        // disable physics
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (pickupSound != null)
            audioSource.PlayOneShot(pickupSound);
    }

    void OnMouseUp()
    {
        pickedUp = false;

        // aktifkan physics lagi
        rb.useGravity = true;

        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
    }
}
