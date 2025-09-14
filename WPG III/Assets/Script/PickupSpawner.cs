using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Prefab Object Clone")]
    public GameObject objectPrefab;    // prefab bungkus mie (atau item lain)
    public Transform playerCamera;     // kamera player
    public float holdDistance = 2f;    // jarak clone dari kamera
    public float smoothSpeed = 10f;    // kehalusan gerak clone

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Kalau player lagi pegang clone
        if (heldObject != null)
        {
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
        // pastikan yang diklik itu hanya SPawner (display mie) → tag khusus
        if (CompareTag("Spawner") && heldObject == null)
        {
            // spawn clone
            heldObject = Instantiate(objectPrefab, playerCamera.position + playerCamera.forward * holdDistance, Quaternion.identity);
            heldRb = heldObject.GetComponent<Rigidbody>();

            // kasih tag khusus clone biar beda dengan spawner
            heldObject.tag = "Clone";

            // disable gravity saat dipegang
            heldRb.useGravity = false;
            heldRb.velocity = Vector3.zero;
            heldRb.angularVelocity = Vector3.zero;
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldRb.useGravity = true;
            heldObject = null;
            heldRb = null;
        }
    }
}
