using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // daftar prefab mobil
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    public float spawnInterval = 30f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 0f, spawnInterval);
    }

    void SpawnCar()
    {
        bool spawnFromLeft = Random.value > 0.5f; // true = kiri, false = kanan
        Transform spawnPoint = spawnFromLeft ? leftSpawnPoint : rightSpawnPoint;

        int randomIndex = Random.Range(0, carPrefabs.Length);
        GameObject car = Instantiate(carPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);

        CarMovement move = car.GetComponent<CarMovement>();
        move.moveRight = spawnFromLeft; // kalau dari kiri, maju ke kanan
    }
}
