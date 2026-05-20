using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GenderuwoController : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTarget;

    [Header("Settings")]
    public float agentSpeed = 3.5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeed;
    }

    void Update()
    {
        // Genderuwo tetap mengejar Player menggunakan NavMesh
        if (playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);
        }
    }

    // --- BAGIAN YANG MENIRU CARMOVEMENT ---
    private void OnTriggerEnter(Collider other)
    {
        // Jika Genderuwo menabrak objek dengan tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tertangkap Genderuwo! Game Over!");

            // Opsional: Matikan kontrol player sebelum pindah scene
            if (other.GetComponent<CharacterController>() != null)
            {
                other.GetComponent<CharacterController>().enabled = false;
            }

            // Pindah ke scene Game Over
            SceneManager.LoadScene("GameOverCrash");
        }
    }
}