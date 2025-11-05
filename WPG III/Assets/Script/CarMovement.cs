using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool moveRight = true;
    public float despawnDistance = 100f; // jarak dari posisi awal di mana mobil akan hilang

    private Vector3 startPos;
    private Vector3 moveDirection;

    void Start()
    {
        // Simpan posisi awal untuk acuan despawn
        startPos = transform.position;

        // Jika prefab ini hadapnya kebalik (misal pickup hadap Z-),
        // kamu bisa ubah arah gerak default di sini
        // contoh: kalau model hadap ke Z-, ganti moveDirection ke Vector3.back
        moveDirection = Vector3.forward; // default arah maju Z+

        // Balik arah mobil jika moveRight = false
        if (!moveRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void Update()
    {
        // Gerak mobil berdasarkan arah hadap model
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);

        // Hitung jarak dari posisi awal
        float distanceTravelled = Vector3.Distance(startPos, transform.position);

        // Hapus mobil kalau sudah terlalu jauh
        if (distanceTravelled > despawnDistance)
            Destroy(gameObject);
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
