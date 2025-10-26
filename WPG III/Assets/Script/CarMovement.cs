using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool moveRight = true;
    public float despawnDistance = 60f; // jarak dari posisi awal di mana mobil akan hilang

    private Vector3 startPos;

    void Start()
    {
        // Simpan posisi awal untuk acuan despawn
        startPos = transform.position;

        // Balik arah mobil jika moveRight = false
        if (!moveRight)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void Update()
    {
        // Gerak maju berdasarkan arah hadap mobil
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Hitung jarak dari posisi awal
        float distanceTravelled = Vector3.Distance(startPos, transform.position);

        // Hapus mobil kalau sudah terlalu jauh
        if (distanceTravelled > despawnDistance)
            Destroy(gameObject);
    }
}
