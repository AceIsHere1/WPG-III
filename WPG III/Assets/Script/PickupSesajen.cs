using UnityEngine;

public class PickupSesajen : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float pickupRange = 5f;
    public float holdDistance = 2f;
    public float smoothSpeed = 20f;

    [Header("Pickup Assist")]
    public float pickupAssistRadius = 0.35f;

    [Header("Trash Settings")]
    public string trashTag = "Trash";
    public float trashRange = 2f;

    private Rigidbody rb;
    private bool isHeld = false;
    private static PickupSesajen currentlyHeld;

    private Vector3 heldRotation = new Vector3(0f, 0f, 0f); // set per prefab in Start

    private int heldLayer;
    private int unheldLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        heldLayer = LayerMask.NameToLayer("HeldObject");
        unheldLayer = LayerMask.NameToLayer("UnheldObject");

        heldRotation = new Vector3(0f, 0f, 0f); // change per prefab
    }

    private void FixedUpdate()
    {
        if (isHeld && currentlyHeld == this)
        {
            if (playerCamera != null && rb != null)
            {
                Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
                Vector3 delta = targetPos - rb.position;

                rb.velocity = delta * smoothSpeed;
                rb.angularVelocity = Vector3.zero;

                Quaternion targetRot = playerCamera.rotation * Quaternion.Euler(heldRotation);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * smoothSpeed));
            }
        }
    }

    private void Update()
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

    private void TryPickup()
    {
        if (currentlyHeld != null) return;
        if (playerCamera == null) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            PickupSesajen pickup = hit.collider.GetComponentInParent<PickupSesajen>();
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
            PickupSesajen pickup = col.GetComponentInParent<PickupSesajen>();
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

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        gameObject.layer = heldLayer;
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;

        isHeld = false;
        currentlyHeld = null;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }

        gameObject.layer = unheldLayer;
    }

    private void TryThrowToTrash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, trashRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag(trashTag))
            {
                Debug.Log("Sesajen dibuang ke sampah!");
                GameEvents.RaiseSesajenDisposed();
                Destroy(gameObject);
                currentlyHeld = null;
                return;
            }
        }

        Debug.Log("Tidak ada tong sampah di dekatmu!");
    }

    public void ForcePickup()
    {
        if (currentlyHeld != null) currentlyHeld.ForceDrop();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        isHeld = true;
        currentlyHeld = this;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        gameObject.layer = heldLayer;
    }

    public void ForceDrop()
    {
        if (currentlyHeld == this)
        {
            isHeld = false;
            currentlyHeld = null;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }

        gameObject.layer = unheldLayer;
    }

    public static PickupSesajen GetCurrentlyHeld()
    {
        return currentlyHeld;
    }

    public bool IsHeld()
    {
        return isHeld;
    }
}