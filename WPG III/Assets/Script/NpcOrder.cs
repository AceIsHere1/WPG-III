using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcOrder : MonoBehaviour
{
    [Header("Order Settings")]
    public string requiredTag = "CookedNoodle";
    public Transform orderPoint;
    public float giveDistance = 2f;

    [Header("Varian Pesanan")]
    public VarianMie pesananNPC; // Sekarang nilai ini akan diacak otomatis saat mulai

    private NpcDialog npcDialog;
    private MoveNPC moveNPC;
    private bool hasReceived = false;

    void Start()
    {
        moveNPC = GetComponent<MoveNPC>();
        npcDialog = GetComponent<NpcDialog>();

        // --- BAGIAN YANG DITAMBAHKAN: MENGACAK PESANAN NPC ---
        // Menghitung jumlah total varian di dalam enum
        int jumlahVarian = System.Enum.GetValues(typeof(VarianMie)).Length;

        // Mengacak dari index 0 sampai sebelum index terakhir (BelumAdaIsi)
        // Random.Range untuk integer batas atasnya bersifat eksklusif (tidak ikut teracak)
        pesananNPC = (VarianMie)Random.Range(0, jumlahVarian - 1);
        // -----------------------------------------------------

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

        // Cek apakah NPC sudah di depan warung
        if (moveNPC.currentState != NPCState.Waiting) return;

        Pickup held = Pickup.GetCurrentlyHeld();

        // Cek apakah player bawa objek mie
        if (held != null && held.CompareTag(requiredTag))
        {
            Transform player = Camera.main.transform;
            float dist = Vector3.Distance(player.position, orderPoint.position);

            if (dist <= giveDistance && Input.GetKeyDown(KeyCode.E))
            {
                // Ambil data isi mangkuk
                MangkokData isiMangkok = held.GetComponent<MangkokData>();
                ReceiveFood(held.gameObject, isiMangkok);
            }
        }
    }

    private void ReceiveFood(GameObject bowl, MangkokData isiMangkok)
    {
        if (bowl == null) return;

        Pickup held = bowl.GetComponent<Pickup>();
        if (held != null) held.ForceDrop();

        // --- PASANG CCTV / DEBUG LOG DI SINI ---
        Debug.Log("=== CEK PESANAN NPC ===");
        Debug.Log("1. NPC Minta Varian: " + pesananNPC);

        if (isiMangkok != null)
        {
            Debug.Log("2. Mangkuk dari Player Berisi: " + isiMangkok.isiMieSaatIni);
        }
        else
        {
            Debug.LogError("FATAL: Komponen 'MangkokData' TIDAK KETEMU di objek mangkuk yang dikasih!");
        }
        Debug.Log("=======================");
        // ---------------------------------------

        // LOGIKA PENGECEKAN PESANAN BENAR ATAU SALAH
        if (isiMangkok != null && isiMangkok.isiMieSaatIni == pesananNPC)
        {
            Debug.Log("Pesanan BENAR!");
            if (npcDialog != null) npcDialog.ShowDialog("Pelanggan: Terimakasih bang! Pas mantap!");
            GameEvents.RaiseNpcServed();
        }
        else
        {
            Debug.Log("Pesanan SALAH!");
            if (npcDialog != null) npcDialog.ShowDialog("Pelanggan: Loh pesenan saya bukan ini! Saya gamau!");
            // Nanti kamu bisa tambah event kurangi duit di sini
        }

        if (bowl.scene.IsValid()) Destroy(bowl);

        hasReceived = true;
        moveNPC.StartReturning(); // NPC disuruh pulang
    }
}