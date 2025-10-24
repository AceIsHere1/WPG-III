using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("References")]
    [SerializeField] private SajenSpawnerGame sesajenSpawner;
    [SerializeField] private NPCSpawner npcSpawner;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform player;

    [Header("Timing")]
    [SerializeField] private float spawnDelay = 2f;

    private GameObject currentGhost;
    private bool ghostSpawned = false;
    private bool spawnPending = false; // mencegah Invoke ganda

    private void OnEnable()
    {
        NPCEvents.OnNpcDestroyed += HandleNpcDestroyed;
        GameEvents.OnSesajenDisposed += HandleSesajenDisposed;
    }

    private void OnDisable()
    {
        NPCEvents.OnNpcDestroyed -= HandleNpcDestroyed;
        GameEvents.OnSesajenDisposed -= HandleSesajenDisposed;
        CancelInvoke(); // hentikan semua Invoke yang masih jalan
    }

    private void HandleNpcDestroyed()
    {
        if (ghostSpawned || spawnPending) return; // sudah ada hantu atau sedang dijadwalkan
        spawnPending = true;

        Invoke(nameof(SpawnGhost), spawnDelay);
    }

    private void SpawnGhost()
    {
        spawnPending = false; // reset flag
        if (ghostSpawned) return; // pastikan tidak double spawn
        ghostSpawned = true;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Tidak ada spawn point untuk hantu!");
            return;
        }

        // Pilih spawn point acak
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawn = spawnPoints[randomIndex];

        // Nonaktifkan NPCSpawner
        if (npcSpawner != null)
        {
            npcSpawner.CancelInvoke();
            npcSpawner.enabled = false;
            Debug.Log("NPCSpawner dinonaktifkan karena hantu muncul");
        }

        // Spawn hantu
        currentGhost = Instantiate(ghostPrefab, randomSpawn.position, randomSpawn.rotation);
        var ghostAI = currentGhost.GetComponent<GhostAI>();

        if (ghostAI != null)
        {
            ghostAI.patrolPoints = patrolPoints;
            ghostAI.player = player;
        }

        Debug.Log($"Hantu muncul di titik {randomSpawn.name}");

        // Spawn sesajen
        if (sesajenSpawner != null)
        {
            sesajenSpawner.SpawnNewSesajen();
            Debug.Log("Sesajen muncul");
        }
    }

    private void HandleSesajenDisposed()
    {
        if (currentGhost != null)
        {
            var ghostAI = currentGhost.GetComponent<GhostAI>();
            if (ghostAI != null)
            {
                // Pastikan musik kejar dimatikan sebelum hantu dihancurkan
                var audio = ghostAI.GetComponent<AudioSource>();
                if (audio != null && audio.isPlaying)
                    audio.Stop();
            }

            Destroy(currentGhost);
            Debug.Log("Hantu menghilang setelah sesajen dibuang!");
        }

        // Bersihkan semua sisa hantu di scene
        foreach (var g in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            Destroy(g);
        }

        // Aktifkan kembali NPCSpawner
        if (npcSpawner != null)
        {
            npcSpawner.CancelInvoke();
            npcSpawner.enabled = true;
            npcSpawner.Invoke(nameof(npcSpawner.SpawnNPC), 2f);
            Debug.Log("NPCSpawner diaktifkan kembali setelah sesajen dibuang");
        }

        ghostSpawned = false;
        spawnPending = false;
    }
}
