using UnityEngine;

public class TutorialGhostSpawner : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private GameObject ghostPrefab;

    [Header("References")]
    [SerializeField] private Transform patrolStartPoint;  // Titik spawn hantu (PatrolPoint_4)
    [SerializeField] private Transform[] patrolPoints;     // Jalur patrol
    [SerializeField] private Transform player;
    [SerializeField] private GameObject sesajen;           // Sesajen yang aktif bersamaan
    [SerializeField] private GameObject trashBin;          // Tong sampah untuk buang sesajen

    private GameObject currentGhost;
    private bool ghostSpawned = false;

    // Dipanggil dari TutorialManager saat Bu Inah bilang "Hmmm enak, sip."
    public void SpawnGhost()
    {
        if (ghostSpawned) return;
        ghostSpawned = true;

        if (ghostPrefab == null || patrolStartPoint == null)
        {
            Debug.LogWarning("Ghost prefab atau patrol start point belum di-assign!");
            return;
        }

        // Spawn hantu di PatrolPoint_4
        currentGhost = Instantiate(ghostPrefab, patrolStartPoint.position, patrolStartPoint.rotation);
        var ghostAI = currentGhost.GetComponent<GhostAITutorial>();

        if (ghostAI != null)
        {
            ghostAI.patrolPoints = patrolPoints;
            ghostAI.player = player;
        }

        Debug.Log("Hantu muncul di PatrolPoint_4!");

        // Aktifkan sesajen dan tong sampah
        if (sesajen != null) sesajen.SetActive(true);
        if (trashBin != null) trashBin.SetActive(true);
    }

    // Dipanggil dari TutorialManager saat sesajen sudah dibuang
    public void DespawnGhost()
    {
        if (currentGhost != null)
        {
            var ghostAI = currentGhost.GetComponent<GhostAI>();
            if (ghostAI != null)
            {
                // Hentikan suara hantu jika ada
                var audio = ghostAI.GetComponent<AudioSource>();
                if (audio != null && audio.isPlaying)
                    audio.Stop();
            }

            Destroy(currentGhost);
            Debug.Log("Hantu menghilang setelah sesajen dibuang!");
        }

        ghostSpawned = false;
    }
}
