using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance; // singleton global

    [Header("Game Settings")]
    [SerializeField] private int npcToWin = 10; // target NPC yang harus dilayani

    private int servedNpcCount = 0;
    private bool gameWon = false;

    public int ServedNpcCount => servedNpcCount;
    public bool GameWon => gameWon;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnNpcServed += HandleNpcServed; // dengarkan event “NPC sudah dilayani”
    }

    private void OnDisable()
    {
        GameEvents.OnNpcServed -= HandleNpcServed;
    }

    private void HandleNpcServed()
    {
        if (gameWon) return;

        servedNpcCount++;
        Debug.Log($"NPC ke-{servedNpcCount} sudah dilayani.");

        if (servedNpcCount >= npcToWin)
        {
            HandleGameWin();
        }
    }

    private void HandleGameWin()
    {
        gameWon = true;
        Debug.Log("Malam pertama selesai! Semua NPC sudah dilayani!");

        // Matikan sistem lain
        var npcSpawner = FindObjectOfType<NPCSpawner>();
        if (npcSpawner) npcSpawner.enabled = false;

        var ghostSpawner = FindObjectOfType<GhostSpawner>();
        if (ghostSpawner) ghostSpawner.enabled = false;

        // Trigger event global (bisa buat munculkan UI kemenangan)
        GameEvents.RaiseGameWon();
    }
}
