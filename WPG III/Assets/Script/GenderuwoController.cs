using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Wajib ditambahkan untuk memanggil VideoPlayer

public class GenderuwoController : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTarget;

    [Header("Monster Movement Settings")]
    public float agentSpeed = 4.5f;
    public float agentAcceleration = 40f;
    public float agentAngularSpeed = 720f;
    public bool disableAutoBraking = true;

    [Header("Jumpscare Settings")]
    [Tooltip("Masukkan komponen VideoPlayer yang berisi video jumpscare")]
    public VideoPlayer jumpscareVideo;
    public string gameOverSceneName = "GameOverCrash";

    private NavMeshAgent agent;
    private bool hasCaughtPlayer = false; // Mencegah jumpscare keputar berkali-kali

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = agentSpeed;
        agent.acceleration = agentAcceleration;
        agent.angularSpeed = agentAngularSpeed;

        if (disableAutoBraking)
        {
            agent.autoBraking = false;
        }

        // Pastikan video jumpscare dimatikan di awal game
        if (jumpscareVideo != null)
        {
            jumpscareVideo.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Berhenti mengejar jika sudah menangkap player
        if (hasCaughtPlayer) return;

        if (playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika menabrak Player dan belum pernah menangkapnya
        if (other.CompareTag("Player") && !hasCaughtPlayer)
        {
            hasCaughtPlayer = true; // Kunci status agar tidak ter-trigger ganda
            Debug.Log("Player tertangkap! Memulai jumpscare...");

            // 1. Hentikan NavMeshAgent seketika
            if (agent != null) agent.isStopped = true;

            // 2. Matikan kontrol player agar tidak bisa kabur saat jumpscare
            if (other.GetComponent<CharacterController>() != null)
            {
                other.GetComponent<CharacterController>().enabled = false;
            }

            // 3. Mainkan Jumpscare
            if (jumpscareVideo != null)
            {
                jumpscareVideo.gameObject.SetActive(true);
                jumpscareVideo.Play();

                // Menyuruh Unity memanggil fungsi OnJumpscareFinished saat video selesai diputar
                jumpscareVideo.loopPointReached += OnJumpscareFinished;
            }
            else
            {
                // Kalau kamu lupa masukin video di Inspector, langsung pindah scene aja
                SceneManager.LoadScene(gameOverSceneName);
            }
        }
    }

    // Fungsi ini otomatis terpanggil saat video jumpscare selesai
    void OnJumpscareFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}