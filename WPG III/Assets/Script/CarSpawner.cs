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

        // Spawn mobil
        GameObject car = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        CarMovement movement = car.GetComponent<CarMovement>();

        // Tentukan arah berdasarkan nama spawn point
        // Misalnya spawn point kanan diberi nama "RightSpawn" dan kiri "LeftSpawn"
        if (spawnPoint.name.ToLower().Contains("right"))
        {
            movement.moveRight = false; // dari kanan ke kiri
        }
        else if (spawnPoint.name.ToLower().Contains("left"))
        {
            movement.moveRight = true; // dari kiri ke kanan
        }
    }
}
