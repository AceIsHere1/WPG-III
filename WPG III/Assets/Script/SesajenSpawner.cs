using UnityEngine;

public class SesajenSpawner : MonoBehaviour
{
    public GameObject sesajenPrefab;
    public Transform[] spawnPoints;

    private GameObject currentSesajen;
    private int lastIndex = -1;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        SpawnNewSesajen();
    }

    public void SpawnNewSesajen()
    {
        if (currentSesajen != null)
        {
            Destroy(currentSesajen);
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        } while (randomIndex == lastIndex); // hindari muncul di titik sama berturut-turut

        lastIndex = randomIndex;

        Transform spawnPoint = spawnPoints[randomIndex];
        currentSesajen = Instantiate(sesajenPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
