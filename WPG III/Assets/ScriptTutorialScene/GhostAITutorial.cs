using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
    public AudioSource chaseMusic;

    [Header("Jumpscare Settings")]
    public VideoPlayer jumpscareVideo;
    public string gameOverSceneName = "TutorialScene";
    private bool hasPlayedJumpscare = false;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (jumpscareVideo == null)
            jumpscareVideo = GameObject.Find("JumpscareVideoTutorial")?.GetComponent<VideoPlayer>();

        if (patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        if (animator != null)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", true);
        }

        if (chaseMusic != null)
            chaseMusic.Stop();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (!isChasing)
            {
                isChasing = true;
                StartChaseMusic();
                UpdateAnimationState(true);
            }
        }
        else if (distanceToPlayer > detectionRadius * 1.5f)
        {
            if (isChasing)
            {
                isChasing = false;
                StopChaseMusic();
                UpdateAnimationState(false);
            }
        }

        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            if (distanceToPlayer <= catchDistance)
            {
                GameOver();
            }
        }
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
        if (hasPlayedJumpscare) return;
        hasPlayedJumpscare = true;

        Debug.Log("Player tertangkap! Memulai jumpscare...");

        agent.isStopped = true;
        StopChaseMusic();

        if (player.GetComponent<CharacterController>() != null)
            player.GetComponent<CharacterController>().enabled = false;
        if (player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().enabled = false;

        if (jumpscareVideo != null)
        {
            jumpscareVideo.gameObject.SetActive(true);
            jumpscareVideo.Play();
            jumpscareVideo.loopPointReached += OnJumpscareFinished;
        }
        else
        {
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
        StopChaseMusic();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

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