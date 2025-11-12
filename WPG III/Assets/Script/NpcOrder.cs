using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcOrder : MonoBehaviour
{
    [Header("Order Settings")]
    public string requiredTag = "CookedNoodle";   // tag mangkok mie jadi
    public Transform orderPoint;                  // tempat player kasih mie
    public float giveDistance = 2f;               // jarak player bisa kasih mie
    private NpcDialog npcDialog;

    [Header("References")]
    private MoveNPC moveNPC;
    private bool hasReceived = false;

    void Start()
    {
        moveNPC = GetComponent<MoveNPC>();
        npcDialog = GetComponent<NpcDialog>();

        if (moveNPC == null) Debug.LogError("MoveNPC tidak ditemukan di NPC!");

        // Ambil order point dari manager di scene
        if (DestinationManager.Instance != null)
        {
            orderPoint = DestinationManager.Instance.orderPoint;
        }
    }

    void Update()
    {
        if (hasReceived) return;

        // cek apakah player pegang mangkok mie jadi
        Pickup held = Pickup.GetCurrentlyHeld();
        if (held != null && held.CompareTag(requiredTag))
        {
            // cek jarak antara player dengan NPC
            Transform player = Camera.main.transform; // asumsi kamera = player
            float dist = Vector3.Distance(player.position, orderPoint.position);

            if (dist <= giveDistance && Input.GetKeyDown(KeyCode.E))
            {
                ReceiveFood(held.gameObject);
            }
        }
    }

    private void ReceiveFood(GameObject bowl)
    {
        if (bowl == null)
        {
            Debug.LogWarning("ReceiveFood dipanggil dengan bowl == null");
            return;
        }

        // Drop dari tangan player jika masih dipegang
        Pickup held = bowl.GetComponent<Pickup>();
        if (held != null)
            held.ForceDrop();

        // Safety: hanya Destroy jika object ini adalah instance di scene.
        // Ini mencegah kita tidak sengaja mencoba menghancurkan prefab asset yang salah assign di inspector.
        if (bowl.scene.IsValid())
        {
            Destroy(bowl);
        }
        else
        {
            Debug.LogWarning($"NpcOrder: coba Destroy object yang bukan bagian scene (mungkin prefab asset). Lewati Destroy untuk: {bowl.name}");
        }

        Debug.Log("NPC menerima mie jadi!");

        // Tampilkan dialog terima kasih
        if (npcDialog != null)
            npcDialog.ShowDialog("Pelanggan: Makasih bang!");

        // lanjutkan perjalanan NPC
        hasReceived = true;
        GameEvents.RaiseNpcServed();
        moveNPC.StartReturning();
    }
}
