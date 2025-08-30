using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupRange = 3f;   // jarak bisa ambil
    public LayerMask pickupLayer;    // layer objek yang bisa diambil
    public Transform holdParent;     // tempat objek ditaruh (misal depan kamera)

    private GameObject heldObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // klik kiri
        {
            if (heldObject == null)
            {
                // coba ambil objek
                TryPickup();
            }
            else
            {
                // kalau sudah pegang → lepas
                Drop();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            heldObject = hit.collider.gameObject;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.transform.position = holdParent.position;
            heldObject.transform.parent = holdParent;
        }
    }

    void Drop()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.parent = null;
        heldObject = null;
    }
}
