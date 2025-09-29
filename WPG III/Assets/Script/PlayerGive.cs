using UnityEngine;

public class PlayerGive : MonoBehaviour
{
    [Header("Settings")]
    public GameObject miePrefab;      // prefab mie
    public Transform handPoint;       // posisi mie di tangan player
    private GameObject heldMie;       // mie yang sedang dipegang

    void Update()
    {
        // Tekan E untuk kasih ke NPC
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldMie != null)
            {
                TryGiveMie();
            }
        }
    }

    // Ambil mie (misal dipanggil saat masak selesai)
    public void TakeMie()
    {
        if (heldMie == null)
        {
            heldMie = Instantiate(miePrefab, handPoint.position, handPoint.rotation, handPoint);
            heldMie.transform.SetParent(handPoint);
        }
    }

    void TryGiveMie()
    {
        // cek apakah ada NPC di trigger area
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hitColliders)
        {
            NPCReceive npc = hit.GetComponent<NPCReceive>();
            if (npc != null)
            {
                npc.ReceiveMie();
                Destroy(heldMie);
                heldMie = null;
                return;
            }
        }
    }
}
