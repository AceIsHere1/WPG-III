using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] private GameObject[] npcPrefabs;    // daftar prefab NPC
    [SerializeField] private Transform[] spawnPoints;    // beberapa titik spawn
    [SerializeField] private float spawnDelay = 2f;      // jeda sebelum NPC baru muncul

    private GameObject currentNPC;                       // referensi NPC yang sedang aktif
    private bool hasSpawnedInitial = false;

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
        // Spawn NPC baru hanya setelah NPC sebelumnya benar-benar hancur
        currentNPC = null;
        Invoke(nameof(SpawnNPC), spawnDelay);
    }

    public void SpawnNPC()
    {
        // Jangan spawn kalau masih ada NPC aktif
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

        // Pilih NPC dan titik spawn acak
        int npcIndex = Random.Range(0, npcPrefabs.Length);
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenPoint = spawnPoints[pointIndex];

        // Spawn NPC baru dan simpan referensinya
        currentNPC = Instantiate(npcPrefabs[npcIndex], chosenPoint.position, chosenPoint.rotation);
        Debug.Log($"Spawn NPC {npcIndex} di titik {pointIndex} ({chosenPoint.name})");
    }
}
