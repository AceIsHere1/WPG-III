using UnityEngine;
using UnityEngine.AI;

public class GenderuwoController : MonoBehaviour
{
    public Transform playerTarget;
    public float catchDistance = 1.5f; // Ubah jarak tangkap di sini

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);

            // Mengecek jarak antara Genderuwo dan Player
            float distance = Vector3.Distance(transform.position, playerTarget.position);

            if (distance <= catchDistance)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER! Tertangkap!");
        Time.timeScale = 0; // Game berhenti
    }
}