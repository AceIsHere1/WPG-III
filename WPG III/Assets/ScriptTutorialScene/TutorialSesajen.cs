using UnityEngine;

public class TutorialSesajen : MonoBehaviour
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
    private static TutorialSesajen currentlyHeld;

    private Vector3 heldRotation = new Vector3(0f, 0f, 0f); // change per prefab in Start

    private int heldLayer;
    private int unheldLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        heldLayer = LayerMask.NameToLayer("HeldObject");
        unheldLayer = LayerMask.NameToLayer("UnheldObject");

        heldRotation = new Vector3(0f, 0f, 0f); // change per prefab
    }

    void FixedUpdate()
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

    void Update()
    {
        if (isHeld && currentlyHeld == this)
        {
            if (Input.GetMouseButtonDown(1)) Drop();
            if (Input.GetKeyDown(KeyCode.E)) TryThrowToTrash();

            // Debug rotation nudging - remove once you find the right angle
            if (Input.GetKey(KeyCode.Keypad8)) heldRotation.x += 1f;
            if (Input.GetKey(KeyCode.Keypad2)) heldRotation.x -= 1f;
            if (Input.GetKey(KeyCode.Keypad4)) heldRotation.y += 1f;
            if (Input.GetKey(KeyCode.Keypad6)) heldRotation.y -= 1f;
            if (Input.GetKey(KeyCode.Keypad7)) heldRotation.z += 1f;
            if (Input.GetKey(KeyCode.Keypad9)) heldRotation.z -= 1f;
            if (Input.GetKeyDown(KeyCode.Keypad5))
                Debug.Log("heldRotation: " + heldRotation);
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

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        SetLayerRecursively(gameObject, heldLayer);
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

        SetLayerRecursively(gameObject, unheldLayer);
    }

    private void TryThrowToTrash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, trashRange);

        foreach (var col in colliders)
        {
            if (col.CompareTag(trashTag))
            {
                Debug.Log("Sesajen dibuang ke sampah!");

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

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
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