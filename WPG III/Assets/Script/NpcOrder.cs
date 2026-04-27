using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcOrder : MonoBehaviour
{
    [Header("Order Settings")]
    public string requiredTag = "CookedNoodle";
    public Transform orderPoint;
    public float giveDistance = 2f;

    private NpcDialog npcDialog;
    private MoveNPC moveNPC;
    private bool hasReceived = false;

    void Start()
    {
        moveNPC = GetComponent<MoveNPC>();
        npcDialog = GetComponent<NpcDialog>();

        if (moveNPC == null) Debug.LogError("MoveNPC tidak ditemukan di NPC!");

        if (DestinationManager.Instance != null)
        {
            orderPoint = DestinationManager.Instance.orderPoint;
        }
    }

    void Update()
    {
        if (hasReceived) return;
        if (moveNPC == null) return;

        // NPC harus sudah Waiting dulu, baru bisa terima mie
        if (moveNPC.currentState != NPCState.Waiting) return;

        Pickup held = Pickup.GetCurrentlyHeld();
        if (held != null && held.CompareTag(requiredTag))
        {
            Transform player = Camera.main.transform;
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

        Pickup held = bowl.GetComponent<Pickup>();
        if (held != null)
            held.ForceDrop();

        if (bowl.scene.IsValid())
        {
            Destroy(bowl);
        }
        else
        {
            Debug.LogWarning($"NpcOrder: coba Destroy object yang bukan bagian scene. Lewati Destroy untuk: {bowl.name}");
        }

        Debug.Log("NPC menerima mie jadi!");

        if (npcDialog != null)
            npcDialog.ShowDialog("Pelanggan: Makasih bang!");

        hasReceived = true;
        GameEvents.RaiseNpcServed();
        moveNPC.StartReturning();
    }
}