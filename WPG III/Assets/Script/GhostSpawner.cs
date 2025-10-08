using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] NPCSpawner npcSpawner;
    [SerializeField] Transform[] patrolPoints;  
    [SerializeField] Transform player;           
    [SerializeField] float spawnDelay = 2f;

    private GameObject currentGhost;
    private bool ghostSpawned = false;

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
        Invoke(nameof(SpawnGhost), spawnDelay);
    }

    private void SpawnGhost()
    {
        if (npcSpawner != null)
        {
            npcSpawner.CancelInvoke();
            npcSpawner.enabled = false;
            Debug.Log("NPCSpawner dinonaktifkan karena hantu muncul");
        }

        currentGhost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
        var ghostAI = currentGhost.GetComponent<GhostAI>();

        // otomatis isi patrol point dan player
        if (ghostAI != null)
        {
            ghostAI.patrolPoints = patrolPoints;
            ghostAI.player = player;
        }

        Debug.Log("Hantu muncul!");
    }

    private void HandleSesajenDisposed()
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost);
            Debug.Log("Hantu menghilang setelah sesajen dibuang!");
        }

        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (var g in allGhosts)
        {
            Destroy(g);
            Debug.Log("Hantu di scene dihancurkan: " + g.name);
        }

        if (npcSpawner != null)
        {
            npcSpawner.enabled = true;
            npcSpawner.CancelInvoke();
            npcSpawner.Invoke(nameof(npcSpawner.SpawnNPC), 2f);
            Debug.Log("NPCSpawner diaktifkan kembali.");
        }

        ghostSpawned = false;
    }
}
