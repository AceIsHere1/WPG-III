using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] npcPrefabs; // daftar prefab NPC berbeda
    [SerializeField] Transform spawnPoint;    // posisi spawn
    [SerializeField] float spawnDelay = 2f;   // delay sebelum spawn NPC baru

    public void SpawnNPC()
    {
        int index = Random.Range(0, npcPrefabs.Length); // pilih NPC random
        Instantiate(npcPrefabs[index], spawnPoint.position, spawnPoint.rotation);
    }

    private void OnEnable()
    {
        // subscribe ke event ketika NPC destroy
        NPCEvents.OnNpcDestroyed += HandleNpcDestroyed;
    }

    private void OnDisable()
    {
        NPCEvents.OnNpcDestroyed -= HandleNpcDestroyed;
    }

    private void HandleNpcDestroyed()
    {
        Invoke(nameof(SpawnNPC), spawnDelay);
    }
}