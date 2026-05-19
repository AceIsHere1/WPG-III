using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // daftar prefab mobil
    public Transform[] spawnPoints; // titik-titik spawn
    public float spawnInterval = 30f; // jeda antar spawn
    public float startDelay = 30f; // delay pertama kali sebelum spawn

    void Start()
    {
        // Jalankan spawn mobil berulang, tapi mulai setelah delay pertama
        InvokeRepeating(nameof(SpawnCar), startDelay, spawnInterval);
    }

    void SpawnCar()
    {
        if (carPrefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        // Pilih prefab dan spawn point random
        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn mobil dengan rotasi PERSIS mengikuti rotasi Spawn Point
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}