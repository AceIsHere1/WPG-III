using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Prefab Object Clone")]
    public GameObject objectPrefab;    // prefab bungkus mie (atau item lain)
    public Transform playerCamera;     // kamera player
    public float holdDistance = 2f;    // jarak clone dari kamera
    public float smoothSpeed = 20f;    // kehalusan gerak clone (increased for faster following)
    public bool useDirectMovement = true; // Toggle for instant vs smooth movement

    private GameObject heldObject;
    private Rigidbody heldRb;
    
    private Quaternion initialRotationOffset;
    
    private Quaternion targetRotation;

    void Start()
    {
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // Kalau player lagi pegang clone
        if (heldObject != null && heldRb != null && playerCamera != null)
        {
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
            
            if (useDirectMovement)
            {
                heldRb.MovePosition(targetPos);
                
                targetRotation = playerCamera.rotation * initialRotationOffset;
                heldRb.MoveRotation(targetRotation);
            }
            else
            {
                heldRb.MovePosition(Vector3.Lerp(heldRb.position, targetPos, Time.fixedDeltaTime * smoothSpeed));
                
                targetRotation = playerCamera.rotation * initialRotationOffset;
                
                heldRb.MoveRotation(Quaternion.Slerp(heldRb.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed));
            }
        }
    }

    void Update()
    {
        // Klik kanan → drop
        if (heldObject != null && Input.GetMouseButtonDown(1))
        {
            DropObject();
        }
    }

    void OnMouseDown()
    {
        // pastikan yang diklik itu hanya Spawner (display mie) → tag khusus
        if (CompareTag("Spawner") && heldObject == null)
        {
            if (playerCamera == null && Camera.main != null)
                playerCamera = Camera.main.transform;
            
            if (playerCamera == null) return;

            // spawn clone
            Vector3 spawnPos = playerCamera.position + playerCamera.forward * holdDistance;
            heldObject = Instantiate(objectPrefab, spawnPos, Quaternion.identity);
            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                heldObject.tag = "Clone";

                heldRb.velocity = Vector3.zero;
                heldRb.angularVelocity = Vector3.zero;
                heldRb.isKinematic = true;
                heldRb.useGravity = false;

                initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * heldObject.transform.rotation;
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null && heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.velocity = Vector3.zero;
            heldRb.angularVelocity = Vector3.zero;
            
            heldObject = null;
            heldRb = null;
        }
    }

    public void ForceDropObject()
    {
        DropObject();
    }

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }
}