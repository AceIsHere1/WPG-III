using UnityEngine;

public class TutorialSesajen : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float pickupRange = 2f;
    public float holdDistance = 2f;
    public float smoothSpeed = 10f;

    [Header("Trash Settings")]
    public string trashTag = "Trash"; // beri tag "Trash" pada objek tong sampah
    public float trashRange = 2f;

    private Rigidbody rb;
    private bool isHeld = false;
    private Transform holder;
    private static TutorialSesajen currentlyHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Assign kamera otomatis (tanpa drag-drop)
        if (playerCamera == null)
        {
            if (Camera.main != null)
            {
                playerCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Tidak menemukan Camera.main di scene!");
            }
        }
    }

    private void Update()
    {
        if (isHeld && currentlyHeld == this)
        {
            HoldPosition();
            if (Input.GetMouseButtonDown(1)) Drop(); // klik kanan lepas
            if (Input.GetKeyDown(KeyCode.E)) TryThrowToTrash(); // tekan E buang ke sampah
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) TryPickup(); // klik kiri ambil
        }
    }

    private void HoldPosition()
    {
        Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
        rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * smoothSpeed));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));
    }

    private void TryPickup()
    {
        if (currentlyHeld != null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.transform == transform)
            {
                isHeld = true;
                currentlyHeld = this;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;

        isHeld = false;
        currentlyHeld = null;
        rb.useGravity = true;
    }

    private void TryThrowToTrash()
    {
        // cek apakah ada tong sampah di dekat player
        Collider[] colliders = Physics.OverlapSphere(transform.position, trashRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag(trashTag))
            {
                Debug.Log("Sesajen dibuang ke sampah!");

                // Hapus hantu yang masih ada di scene
                var ghostSpawner = FindObjectOfType<TutorialGhostSpawner>();
                if (ghostSpawner != null)
                    ghostSpawner.DespawnGhost();

                // Beri tahu TutorialManager kalau sesajen sudah dibuang
                FindObjectOfType<TutorialManager>()?.OnPlayerAction("sesajen_dibuang");

                Destroy(gameObject);
                currentlyHeld = null;
                return;
            }
        }

        Debug.Log("Tidak ada tong sampah di dekatmu!");
    }
}
