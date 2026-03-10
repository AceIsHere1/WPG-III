using UnityEngine;

public class TutorialSesajen : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float pickupRange = 5f;
    public float holdDistance = 2f;
    public float smoothSpeed = 20f;
    public bool useDirectMovement = true;

    [Header("Pickup Assist")]
    public float pickupAssistRadius = 0.35f;

    [Header("Trash Settings")]
    public string trashTag = "Trash";
    public float trashRange = 2f;

    private Rigidbody rb;
    private bool isHeld = false;
    private static TutorialSesajen currentlyHeld;

    private Quaternion initialRotationOffset;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    void FixedUpdate()
    {
        if (isHeld && currentlyHeld == this)
        {
            HoldPosition();
        }
    }

    void Update()
    {
        if (isHeld && currentlyHeld == this)
        {
            if (Input.GetMouseButtonDown(1)) Drop();
            if (Input.GetKeyDown(KeyCode.E)) TryThrowToTrash();
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) TryPickup();
        }
    }

    private void HoldPosition()
    {
        if (playerCamera == null) return;

        Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;

        if (useDirectMovement)
        {
            rb.MovePosition(targetPos);

            targetRotation = playerCamera.rotation * initialRotationOffset;
            rb.MoveRotation(targetRotation);
        }
        else
        {
            rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.fixedDeltaTime * smoothSpeed));

            targetRotation = playerCamera.rotation * initialRotationOffset;

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed));
        }
    }

    private void TryPickup()
    {
        if (currentlyHeld != null) return;
        if (playerCamera == null) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Normal raycast
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            TutorialSesajen pickup = hit.collider.GetComponentInParent<TutorialSesajen>();

            if (pickup == this)
            {
                PerformPickup();
                return;
            }
        }

        // Pickup assist
        Vector3 assistPoint = playerCamera.position + playerCamera.forward * holdDistance;

        Collider[] nearby = Physics.OverlapSphere(assistPoint, pickupAssistRadius);

        foreach (Collider col in nearby)
        {
            TutorialSesajen pickup = col.GetComponentInParent<TutorialSesajen>();

            if (pickup == this)
            {
                PerformPickup();
                return;
            }
        }
    }

    private void PerformPickup()
    {
        isHeld = true;
        currentlyHeld = this;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        if (playerCamera != null)
            initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * transform.rotation;
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;

        isHeld = false;
        currentlyHeld = null;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void TryThrowToTrash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, trashRange);

        foreach (var col in colliders)
        {
            if (col.CompareTag(trashTag))
            {
                Debug.Log("Sesajen dibuang ke sampah!");

                // Tutorial-specific behavior
                var ghostSpawner = FindObjectOfType<TutorialGhostSpawner>();
                if (ghostSpawner != null)
                    ghostSpawner.DespawnGhost();

                FindObjectOfType<TutorialManager>()?.OnPlayerAction("sesajen_dibuang");

                Destroy(gameObject);
                currentlyHeld = null;
                return;
            }
        }

        Debug.Log("Tidak ada tong sampah di dekatmu!");
    }

    public bool IsHeld()
    {
        return isHeld;
    }
    public static TutorialSesajen GetCurrentlyHeld()
{
    return currentlyHeld;
}
}