using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BuInahMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform[] destinations;  // titik tujuan Bu Inah
    [SerializeField] private float stopDistance = 0.5f; // jarak berhenti

    private NavMeshAgent agent;
    private Animator animator;
    private int currentIndex = 0;
    private bool canMove = false; // aktif setelah tutorial selesai

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent tidak ditemukan di Bu Inah!");
            return;
        }

        if (destinations == null || destinations.Length == 0)
        {
            Debug.LogWarning("Bu Inah belum memiliki rute keluar!");
            return;
        }

        // Awalnya diam di tempat (idle)
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        if (!canMove || destinations.Length == 0) return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            if (currentIndex < destinations.Length - 1)
            {
                currentIndex++;
                SetNextDestination();
            }
            else
            {
                // Sampai tujuan terakhir → berhenti dan ganti scene
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);
                canMove = false;
                Debug.Log("Bu Inah sudah pergi dari warung, pindah ke GameScene...");
                StartCoroutine(LoadGameScene());
            }
        }
    }

    private void SetNextDestination()
    {
        if (destinations.Length == 0) return;
        agent.isStopped = false;
        agent.SetDestination(destinations[currentIndex].position);
    }

    /// <summary>
    /// Dipanggil oleh TutorialManager saat semua tutorial selesai.
    /// </summary>
    public void StartLeaving()
    {
        if (destinations == null || destinations.Length == 0)
        {
            Debug.LogWarning("Bu Inah tidak memiliki rute keluar!");
            return;
        }

        currentIndex = 0;
        canMove = true;
        SetNextDestination();
        Debug.Log("Bu Inah mulai berjalan pergi...");
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(2f); // beri jeda sebelum pindah scene
        SceneManager.LoadScene("GameScene");
    }
}
