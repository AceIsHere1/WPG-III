using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GenderuwoController : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTarget;

    [Header("Monster Movement Settings")]
    public float agentSpeed = 4.5f;          // Kecepatan maksimal (Top Speed)
    public float agentAcceleration = 40f;    // Seberapa cepat mencapai Top Speed (Default Unity cuma 8)
    public float agentAngularSpeed = 720f;   // Kecepatan belok/berputar (Default Unity cuma 120)
    public bool disableAutoBraking = true;   // Matikan rem otomatis agar tidak melambat saat dekat player

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Menerapkan pengaturan ke NavMeshAgent
        agent.speed = agentSpeed;
        agent.acceleration = agentAcceleration;
        agent.angularSpeed = agentAngularSpeed;

        // Mematikan rem otomatis membuat monster terasa lebih buas dan tidak ragu-ragu
        if (disableAutoBraking)
        {
            agent.autoBraking = false;
        }
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