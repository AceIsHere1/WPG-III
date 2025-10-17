using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] private GameObject[] npcPrefabs;    // daftar prefab NPC
    [SerializeField] private Transform[] spawnPoints;    // beberapa titik spawn
    [SerializeField] private float spawnDelay = 2f;      // jeda sebelum NPC baru muncul

    private GameObject currentNPC;                       // referensi NPC yang sedang aktif
    private bool hasSpawnedInitial = false;

    private int currentNpcIndex = 0;                     // urutan prefab NPC
    private int currentSpawnPointIndex = 0;              // urutan titik spawn

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
        Invoke(nameof(SpawnNPC), spawnDelay);
    }

    public void SpawnNPC()
    {
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

        // Pilih NPC berdasarkan urutan
        int npcIndex = currentNpcIndex;
        currentNpcIndex = (currentNpcIndex + 1) % npcPrefabs.Length; // berputar ke awal lagi

        // Pilih spawn point berdasarkan urutan juga
        int pointIndex = currentSpawnPointIndex;
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

        Transform chosenPoint = spawnPoints[pointIndex];

        // Spawn NPC baru dan simpan referensinya
        currentNPC = Instantiate(npcPrefabs[npcIndex], chosenPoint.position, chosenPoint.rotation);
        Debug.Log($"Spawn NPC {npcIndex + 1} di titik {pointIndex + 1} ({chosenPoint.name})");
    }
}
