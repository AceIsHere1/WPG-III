using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;       // Kamera player
    public float holdDistance = 2f;      // Jarak object dari kamera
    public float smoothSpeed = 10f;      // Kecepatan smoothing biar gak getar
    public float pickupRange = 3f;       // Jarak maksimum bisa ambil

    private bool pickedUp;
    private Rigidbody rb;

    [Header("Sound Settings")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    // hanya 1 object boleh dipegang
    private static Pickup currentlyHeld;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // kalau lupa assign kamera, otomatis ambil Main Camera
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        // setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (pickedUp && currentlyHeld == this)
        {
            // hitung posisi di depan kamera (crosshair)
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;

            // gerakkan object smooth ke targetPos
            rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * smoothSpeed));

            // rotasi ikut kamera
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));

            // klik kanan = drop
            if (Input.GetMouseButtonDown(1))
            {
                Drop();
            }
        }
        else
        {
            // cek input klik kiri untuk pickup
            if (Input.GetMouseButtonDown(0))
            {
                TryPickup();
            }
        }
    }

    private void TryPickup()
    {
        if (currentlyHeld != null) return; // sudah ada object di tangan

        // raycast dari kamera
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.transform == transform) // pastikan ray kena object ini
            {
                pickedUp = true;
                currentlyHeld = this;

                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                if (pickupSound != null)
                    audioSource.PlayOneShot(pickupSound);
            }
        }
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;

        pickedUp = false;
        currentlyHeld = null;

        rb.useGravity = true;

        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
    }
}
