using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] private GameObject[] npcPrefabs;    // daftar prefab NPC
    [SerializeField] private Transform[] spawnPoints;    // beberapa titik spawn
    [SerializeField] private float spawnDelay = 2f;      // jeda sebelum NPC baru muncul

    [Header("References")]
    [SerializeField] private GhostSpawner ghostSpawner;  // referensi ke GhostSpawner

    private GameObject currentNPC;                       // referensi NPC yang sedang aktif
    private bool hasSpawnedInitial = false;

    private int currentNpcIndex = 0;                     // urutan prefab NPC
    private int currentSpawnPointIndex = 0;              // urutan titik spawn
    public bool CanSpawn { get; set; } = true;

    private void OnEnable()
    {
        NPCEvents.OnNpcDestroyed += HandleNpcDestroyed;

        if (!hasSpawnedInitial)
        {
            hasSpawnedInitial = true;
            Invoke(nameof(SpawnNPC), spawnDelay);
        }
    }

    private void OnDisable()
    {
        NPCEvents.OnNpcDestroyed -= HandleNpcDestroyed;
        CancelInvoke();
    }

    private void HandleNpcDestroyed()
    {
        currentNPC = null;

        // Cek dulu apakah hantu sedang aktif
        if (ghostSpawner != null && ghostSpawner.IsGhostActive)
        {
            Debug.Log("Hantu sedang aktif — tunda spawn NPC sampai hantu hilang.");
            return;
        }

        Invoke(nameof(SpawnNPC), spawnDelay);
    }

    public void SpawnNPC()
    {
        // Cegah spawn kalau hantu sedang aktif
        if (ghostSpawner != null && ghostSpawner.IsGhostActive)
        {
            Debug.Log("Hantu masih aktif — tidak bisa spawn NPC baru.");
            return;
        }

        if (currentNPC != null)
        {
            Debug.Log("Masih ada NPC aktif, skip spawn baru.");
            return;
        }

        if (npcPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("NPCSpawner belum diset dengan benar (prefab atau spawn point kosong).");
            return;
        }

        // Pilih NPC dan titik spawn berdasarkan urutan (bisa kamu ubah ke random kalau mau)
        int npcIndex = currentNpcIndex;
        currentNpcIndex = (currentNpcIndex + 1) % npcPrefabs.Length;

        int pointIndex = currentSpawnPointIndex;
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

        Transform chosenPoint = spawnPoints[pointIndex];

        // Spawn NPC baru
        currentNPC = Instantiate(npcPrefabs[npcIndex], chosenPoint.position, chosenPoint.rotation);
        Debug.Log($"Spawn NPC {npcIndex + 1} di titik {pointIndex + 1} ({chosenPoint.name})");
    }
}
