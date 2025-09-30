using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcOrder : MonoBehaviour
{
    [Header("Order Settings")]
    public string requiredTag = "CookedNoodle";   // tag mangkok mie jadi
    public Transform orderPoint;                  // tempat player kasih mie
    public float giveDistance = 2f;               // jarak player bisa kasih mie

    [Header("References")]
    private MoveNPC moveNPC;
    private bool hasReceived = false;

    void Start()
    {
        moveNPC = GetComponent<MoveNPC>();
        if (moveNPC == null) Debug.LogError("MoveNPC tidak ditemukan di NPC!");
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
        // hancurkan mangkok mie dari player
        Pickup held = bowl.GetComponent<Pickup>();
        if (held != null) held.ForceDrop();

        Destroy(bowl);

        Debug.Log("NPC menerima mie jadi!");

        // lanjutkan perjalanan NPC
        hasReceived = true;
        // aktifkan balik NPC untuk jalan pulang
        moveNPC.StartReturning();
    }
}
