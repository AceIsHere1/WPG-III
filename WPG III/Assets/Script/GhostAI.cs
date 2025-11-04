using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GhostAI : MonoBehaviour
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

    [Header("Jumpscare Settings")]
    public VideoPlayer jumpscareVideo; // drag komponen VideoPlayer ke sini
    public string gameOverSceneName = "GameOverScene";
    private bool hasPlayedJumpscare = false;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // cari video jumpscare di scene kalau belum diset dari inspector
        if (jumpscareVideo == null)
            jumpscareVideo = GameObject.Find("JumpscareVideo")?.GetComponent<VideoPlayer>();

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
        if (hasPlayedJumpscare) return; // biar tidak ter-trigger berkali kali
        hasPlayedJumpscare = true;

        Debug.Log("Player tertangkap! Memulai jumpscare...");

        // Hentikan pergerakan ghost dan audio chase
        agent.isStopped = true;
        StopChaseMusic();

        // Nonaktifkan kontrol player (kalau punya script kontrol)
        if (player.GetComponent<CharacterController>() != null)
            player.GetComponent<CharacterController>().enabled = false;
        if (player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().enabled = false;

        // Tampilkan video jumpscare
        if (jumpscareVideo != null)
        {
            jumpscareVideo.gameObject.SetActive(true);
            jumpscareVideo.Play();

            // Setelah video selesai, panggil fungsi pindah scene
            jumpscareVideo.loopPointReached += OnJumpscareFinished;
        }
        else
        {
            // kalau tidak ada video, langsung ke GameOverScene
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    void OnJumpscareFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(gameOverSceneName);
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