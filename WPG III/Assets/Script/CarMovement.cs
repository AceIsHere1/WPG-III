using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f;
    public float despawnDistance = 100f; // jarak dari posisi awal mobil akan hilang

    private Vector3 startPos;

    void Start()
    {
        // Simpan posisi awal untuk acuan despawn
        startPos = transform.position;
    }

    void Update()
    {
        // Gerak mobil selalu maju ke depan (lokal Z axis)
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        // Hitung jarak dari posisi awal
        float distanceTravelled = Vector3.Distance(startPos, transform.position);

        // Hapus mobil kalau sudah terlalu jauh
        if (distanceTravelled > despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika mobil menabrak objek dengan tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tertabrak mobil! Game Over!");
            SceneManager.LoadScene("GameOverCrash");
        }
    }
}