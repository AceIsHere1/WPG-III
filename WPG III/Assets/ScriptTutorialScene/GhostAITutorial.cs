using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhostAITutorial : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 5f;
    private int currentPoint = 0;

    [Header("Chase Settings")]
    public Transform player;
    public float chaseSpeed = 11f;
    public float detectionRadius = 10f;
    public float catchDistance = 1.5f;

    [Header("Audio Settings")]
    public AudioSource chaseMusic; // drag AudioSource di inspector (berisi chase music)

    private NavMeshAgent agent;
    private bool isChasing = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        // set animasi awal ke patrol
        if (animator != null)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", true);
        }

        // pastikan musik dalam keadaan off di awal
        if (chaseMusic != null)
            chaseMusic.Stop();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // deteksi player
        if (distanceToPlayer <= detectionRadius)
        {
            if (!isChasing)
            {
                isChasing = true;
                StartChaseMusic(); // mulai musik kejar
                UpdateAnimationState(true);
            }
        }
        else if (distanceToPlayer > detectionRadius * 1.5f)
        {
            if (isChasing)
            {
                isChasing = false;
                StopChaseMusic(); // berhenti musik kejar
                UpdateAnimationState(false);
            }
        }

        // kejar player
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            if (distanceToPlayer <= catchDistance)
            {
                GameOver();
            }
        }

        // mode patroli
        else
        {
            agent.speed = patrolSpeed;

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void GameOver()
    {
        Debug.Log("Player tertangkap! Game Over!");
        SceneManager.LoadScene("TutorialScene");
    }

    void StartChaseMusic()
    {
        if (chaseMusic != null && !chaseMusic.isPlaying)
            chaseMusic.Play();
    }

    void StopChaseMusic()
    {
        if (chaseMusic != null && chaseMusic.isPlaying)
            chaseMusic.Stop();
    }

    void UpdateAnimationState(bool chasing)
    {
        if (animator == null) return;

        animator.SetBool("isChasing", chasing);
        animator.SetBool("isPatrolling", !chasing);
    }

    void OnDestroy()
    {
        // pastikan musik berhenti kalau hantu dihapus
        StopChaseMusic();
    }

    void OnDrawGizmosSelected()
    {
        // radius deteksi
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // jalur waypoint
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Vector3 from = patrolPoints[i].position;
                Vector3 to = patrolPoints[(i + 1) % patrolPoints.Length].position;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
