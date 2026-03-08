using UnityEngine;

public class PickupSesajen : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float pickupRange = 3f; // Increased to hand's length
    public float holdDistance = 2f;
    public float smoothSpeed = 20f; // Increased for faster following
    public bool useDirectMovement = true; // Toggle for instant vs smooth movement

    [Header("Trash Settings")]
    public string trashTag = "Trash"; // beri tag "Trash" pada objek tong sampah
    public float trashRange = 2f;

    private Rigidbody rb;
    private bool isHeld = false;
    private static PickupSesajen currentlyHeld;
    
    private Quaternion initialRotationOffset;
    
    private Quaternion targetRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (isHeld && currentlyHeld == this)
        {
            HoldPosition();
        }
    }

    private void Update()
    {
        if (isHeld && currentlyHeld == this)
        {
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
        if (playerCamera == null) return;

        Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
        
        if (rb != null)
        {
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
    }

    private void TryPickup()
    {
        if (currentlyHeld != null) return;
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.transform == transform)
            {
                isHeld = true;
                currentlyHeld = this;
                
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                if (playerCamera != null)
                    initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * transform.rotation;
            }
        }
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
        }
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
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (playerCamera != null)
            initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * transform.rotation;
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
        }
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