using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float holdDistance = 2f;
    public float smoothSpeed = 10f;
    public float pickupRange = 3f;      // Jarak maksimum bisa ambil

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
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Kalau object ini yang sedang dipegang
        if (currentlyHeld == this)
        {
            // posisi di depan kamera
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
            rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * smoothSpeed));
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));

            // klik kanan = drop
            if (Input.GetMouseButtonDown(1))
            {
                Drop();
            }
        }
        else
        {
            // cek pickup input
            if (Input.GetMouseButtonDown(0)) // klik kiri ambil
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

        currentlyHeld = null;
        rb.useGravity = true;

        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
    }
}
