using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    private NavMeshAgent agent;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // deteksi player
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectionRadius * 1.5f)
        {
            isChasing = false;
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
        SceneManager.LoadScene("GameOverScene");
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