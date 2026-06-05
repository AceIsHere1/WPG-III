using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum NPCState { Walking, Waiting, Returning }

public class MoveNPC : MonoBehaviour
{
    [SerializeField] Transform[] destinations;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private NpcDialog npcDialog;
    private int currentIndex = 0;
    private bool isReturning = false;

    public NPCState currentState = NPCState.Walking; // ← tambah ini

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        npcDialog = GetComponent<NpcDialog>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent tidak ada di NPC!");
            return;
        }

        if (DestinationManager.Instance != null)
        {
            destinations = DestinationManager.Instance.destinations;
        }

        if (destinations == null || destinations.Length == 0)
        {
            Debug.LogError("Destinations belum di-assign di DestinationManager!");
            return;
        }

        SetDestination();
    }

    void Update()
    {
        if (destinations.Length == 0) return;

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            if (!isReturning)
            {
                if (currentIndex < destinations.Length - 1)
                {
                    currentIndex++;
                    SetDestination();
                    currentState = NPCState.Walking; // ← masih jalan
                }
                else
                {
                    // Sampai tujuan terakhir - nunggu mie
                    navMeshAgent.isStopped = true;
                    currentState = NPCState.Waiting; // ← baru boleh terima mie

                    // --- BAGIAN YANG DIUBAH MULAI DI SINI ---
                    string teksPesanan = "Pelanggan: Pesan mie seporsi bang!"; // Dialog default jaga-jaga
                    NpcOrder dataPesanan = GetComponent<NpcOrder>();

                    if (dataPesanan != null)
                    {
                        // Menyesuaikan dialog dengan enum VarianMie
                        switch (dataPesanan.pesananNPC)
                        {
                            case VarianMie.MieUnguSambalMatah:
                                teksPesanan = "Pelanggan: Bang, pesen Mie Ungu Sambal Matah satu!";
                                break;
                            case VarianMie.MieGorengKeriting:
                                teksPesanan = "Pelanggan: Bang, pesen Mie Goreng Keriting ya!";
                                break;
                            case VarianMie.MieGorengBiasa:
                                teksPesanan = "Pelanggan: Bang, Mie Goreng Biasa aja satu dong!";
                                break;
                            case VarianMie.BelumAdaIsi:
                                teksPesanan = "Pelanggan: Bang, pesen mangkok kosong! (Tolong ini error bang)";
                                break;
                        }
                    }

                    if (npcDialog != null)
                        npcDialog.ShowDialog(teksPesanan);
                    // --- BAGIAN YANG DIUBAH SELESAI DI SINI ---
                }
            }
            else
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    SetDestination();
                    currentState = NPCState.Returning; // ← lagi balik
                }
                else
                {
                    Debug.Log("NPC sampai tujuan, destroy...");
                    NPCEvents.RaiseNpcDestroyed();
                    Destroy(gameObject);
                    GameEvents.RaiseNpcExited();
                }
            }
        }
    }

    private void SetDestination()
    {
        if (destinations.Length == 0) return;
        navMeshAgent.SetDestination(destinations[currentIndex].position);
    }

    public void StartReturning()
    {
        if (destinations.Length == 0) return;
        isReturning = true;
        navMeshAgent.isStopped = false;
        currentState = NPCState.Returning; // ← update state
        currentIndex = destinations.Length - 1;
        SetDestination();
    }
}