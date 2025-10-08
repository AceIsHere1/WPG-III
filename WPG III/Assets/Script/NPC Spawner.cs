using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] private GameObject[] npcPrefabs; // daftar prefab NPC berbeda
    [SerializeField] private Transform spawnPoint;    // posisi spawn
    [SerializeField] private float spawnDelay = 2f;   // delay sebelum spawn NPC baru

    private bool hasSpawnedInitial = false;

    private void OnEnable()
    {
        // Langganan ke event ketika NPC dihancurkan
        NPCEvents.OnNpcDestroyed += HandleNpcDestroyed;

        // Spawn NPC pertama kali saat scene dimulai (setelah delay)
        if (!hasSpawnedInitial)
        {
            hasSpawnedInitial = true;
            Invoke(nameof(SpawnNPC), spawnDelay);
        }
    }

    private void OnDisable()
    {
        NPCEvents.OnNpcDestroyed -= HandleNpcDestroyed;
    }

    private void HandleNpcDestroyed()
    {
        // Tunggu beberapa detik sebelum spawn NPC baru
        Invoke(nameof(SpawnNPC), spawnDelay);
    }

    public void SpawnNPC()
    {
        if (npcPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogWarning("NPCSpawner belum diset dengan benar (prefab atau spawn point kosong).");
            return;
        }

        int index = Random.Range(0, npcPrefabs.Length); // pilih NPC random
        Instantiate(npcPrefabs[index], spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"NPC ke-{index} muncul di {spawnPoint.position}");
    }
}
