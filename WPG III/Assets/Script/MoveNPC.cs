using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveNPC : MonoBehaviour
{
    [SerializeField] Transform[] destinations;  // daftar tujuan
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentIndex = 0;
    private bool isReturning = false;           // apakah NPC sedang balik?

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent tidak ada di NPC!");
            return;
        }

        // ambil destinasi dari DestinationManager
        destinations = DestinationManager.Instance.destinations;

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
                // maju ke depan
                if (currentIndex < destinations.Length - 1)
                {
                    currentIndex++;
                    SetDestination();
                }
                else
                {
                    // sampai tujuan terakhir - berhenti (nunggu mie)
                    navMeshAgent.isStopped = true;
                }
            }
            else
            {
                // balik ke belakang
                if (currentIndex > 0)
                {
                    currentIndex--;
                    SetDestination();
                }
                else
                {
                    // sampai tujuan awal - destroy NPC
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
        Vector3 targetVector = destinations[currentIndex].position;
        navMeshAgent.SetDestination(targetVector);
    }

    // dipanggil dari NpcOrder setelah menerima mie
    public void StartReturning()
    {
        if (destinations.Length == 0) return;

        isReturning = true;
        navMeshAgent.isStopped = false;
        currentIndex = destinations.Length - 1; // mulai dari posisi terakhir (warung)
        SetDestination();
    }
}
