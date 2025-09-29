using UnityEngine;

public class HeldItemHolder : MonoBehaviour
{
    [Tooltip("Tempat item diparent saat dipegang (mis. hand point)")]
    public Transform holdPoint;

    private GameObject heldItem;

    // Dipanggil oleh sistem pickup saat player mengambil item
    public void SetHeldItem(GameObject item)
    {
        heldItem = item;
        if (heldItem != null && holdPoint != null)
        {
            // parent, disable physics agar gak jatuh
            heldItem.transform.SetParent(holdPoint, true);
            heldItem.transform.position = holdPoint.position;
            heldItem.transform.rotation = holdPoint.rotation;

            var rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            // disable collider supaya gak nabrak player saat dipegang (opsional)
            foreach (var col in heldItem.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }
    }

    public GameObject GetHeldItem()
    {
        return heldItem;
    }

    // Dipanggil setelah item diberikan ke NPC
    public void ClearHeldItem()
    {
        if (heldItem == null) return;

        // lepaskan parent dulu (NPCReceive akan re-parent atau destroy)
        heldItem.transform.SetParent(null);

        // kalau ada rigidbody, enable supaya NPC bisa memanage atau supaya object bereaksi lagi (NPCReceive akan override)
        var rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // re-enable collider (opsional) — NPCReceive bisa men-disable lagi
        foreach (var col in heldItem.GetComponentsInChildren<Collider>())
            col.enabled = true;

        heldItem = null;
    }

    public bool HasItem()
    {
        return heldItem != null;
    }
}
