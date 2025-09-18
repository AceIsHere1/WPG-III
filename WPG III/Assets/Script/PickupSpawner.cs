using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Prefab Object Clone")]
    public GameObject objectPrefab;    // prefab bungkus mie (clone)
    public Transform playerCamera;     // kamera player
    public float holdDistance = 2f;    // jarak dari kamera
    public float smoothSpeed = 10f;    // kehalusan gerak object

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Kalau player lagi pegang clone
        if (heldObject != null)
        {
            // posisi dan rotasi smooth di depan kamera
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
            heldRb.MovePosition(Vector3.Lerp(heldRb.position, targetPos, Time.deltaTime * smoothSpeed));
            heldRb.MoveRotation(Quaternion.Lerp(heldRb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));

            // Klik kanan → drop
            if (Input.GetMouseButtonDown(1))
            {
                DropObject();
            }
        }
    }

    void OnMouseDown()
    {
        // klik spawner (dummy mie di rak)
        if (CompareTag("Spawner") && heldObject == null)
        {
            // spawn clone di depan kamera
            heldObject = Instantiate(
                objectPrefab,
                playerCamera.position + playerCamera.forward * holdDistance,
                Quaternion.identity
            );

            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                // clone bisa dipegang → disable gravity
                heldRb.useGravity = false;
                heldRb.isKinematic = false; // biar masih bisa di-drop nanti
                heldRb.velocity = Vector3.zero;
                heldRb.angularVelocity = Vector3.zero;
            }

            // tag khusus clone supaya beda dengan spawner
            heldObject.tag = "Clone";
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            // aktifkan gravity biar jatuh natural
            heldRb.useGravity = true;

            // reset reference
            heldObject = null;
            heldRb = null;
        }
    }
}
