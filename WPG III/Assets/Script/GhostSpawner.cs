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

    [Header("Spawn Chance")]
    [Range(0f, 1f)]
    [SerializeField] private float ghostSpawnChance = 0.3f;
    // peluang 30% hantu muncul setiap kali NPC ter-destroy

    [Header("Audio Settings")]
    [SerializeField] private AudioClip kuntiDatangSFX;  // suara saat hantu muncul
    [SerializeField] private AudioSource audioSource;  // tempat mainin suara

    private GameObject currentGhost;
    private bool ghostSpawned = false;
    public bool IsGhostActive => ghostSpawned;


    private void OnEnable()
    {
        NPCEvents.OnNpcDestroyed += HandleNpcDestroyed;
        GameEvents.OnSesajenDisposed += HandleSesajenDisposed;
    }

    private void OnDisable()
    {
        NPCEvents.OnNpcDestroyed -= HandleNpcDestroyed;
        GameEvents.OnSesajenDisposed -= HandleSesajenDisposed;
    }

    private void HandleNpcDestroyed()
    {
        if (ghostSpawned)
        {
            Debug.Log("Hantu masih aktif, skip spawn baru.");
            return;
        }

        // Acak peluang spawn hantu
        float randomValue = Random.value;
        Debug.Log($"Cek spawn hantu... (random={randomValue:F2}, chance={ghostSpawnChance})");

        if (randomValue <= ghostSpawnChance)
        {
            SpawnGhost();
        }
        else
        {
            // Spawn NPC baru seperti biasa
            if (npcSpawner != null && npcSpawner.enabled)
            {
                npcSpawner.CancelInvoke();
                npcSpawner.Invoke(nameof(npcSpawner.SpawnNPC), 2f);
                Debug.Log("Tidak muncul hantu, spawn NPC baru.");
            }
        }
    }

    private void SpawnGhost()
    {
        ghostSpawned = true;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Tidak ada spawn point untuk hantu!");
            return;
        }

        // Pilih spawn point acak
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawn = spawnPoints[randomIndex];

        // Nonaktifkan NPCSpawner dengan aman
        if (npcSpawner != null)
        {
            npcSpawner.CancelInvoke();
            npcSpawner.CanSpawn = false; // <— tambahkan ini
            npcSpawner.enabled = false;

            // Hancurkan NPC aktif kalau ada
            var existingNPC = GameObject.FindGameObjectWithTag("NPC");
            if (existingNPC != null) Destroy(existingNPC);

            Debug.Log("NPCSpawner dan NPC dimatikan karena hantu muncul");
        }

        // Spawn hantu
        currentGhost = Instantiate(ghostPrefab, randomSpawn.position, randomSpawn.rotation);
        var ghostAI = currentGhost.GetComponent<GhostAI>();

        if (ghostAI != null)
        {
            ghostAI.patrolPoints = patrolPoints;
            ghostAI.player = player;
        }

        Debug.Log("Hantu muncul di titik {randomSpawn.name}");

        // Mainkan suara saat hantu muncul
        if (audioSource != null && kuntiDatangSFX != null)
        {
            audioSource.PlayOneShot(kuntiDatangSFX);
            Debug.Log("Sound 'kunti datang' dimainkan!");
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip belum diset di GhostSpawner!");
        }

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
            Destroy(currentGhost);
            Debug.Log("Hantu menghilang setelah sesajen dibuang!");
        }

        foreach (var g in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            Destroy(g);
        }

        // Aktifkan kembali NPCSpawner
        if (npcSpawner != null)
        {
            npcSpawner.enabled = true;
            npcSpawner.CanSpawn = true; // aktifkan kembali flag
            npcSpawner.CancelInvoke();
            npcSpawner.Invoke(nameof(npcSpawner.SpawnNPC), 2f);
            Debug.Log("NPCSpawner diaktifkan kembali setelah sesajen dibuang");
        }

        ghostSpawned = false;
    }
}
