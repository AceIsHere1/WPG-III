using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NpcInteract npcInteract))
                {
                    npcInteract.Interact();
                    break; // biar nggak spam ke banyak NPC sekaligus
                }
            }
        }
    }
}
