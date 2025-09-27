using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactMask = ~0; // optional: limit layer

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, interactRange, interactMask);
            foreach (var c in cols)
            {
                if (c.TryGetComponent<NpcInteract>(out var npc))
                {
                    npc.Interact();
                    break; // ambil NPC terdekat saja
                }
            }
        }
    }

    // Gizmo opsional supaya gampang testing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
