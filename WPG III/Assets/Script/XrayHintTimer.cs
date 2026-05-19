using UnityEngine;

public class XRayHintTimer : MonoBehaviour
{
    [Header("Pengaturan Material")]
    [Tooltip("Masukkan material XRay buatanmu yang warna merah tadi")]
    public Material materialXRay;

    [Header("Pengaturan Waktu")]
    [Tooltip("Waktu dalam detik sebelum XRay menyala (120 = 2 menit)")]
    public float waktuTunggu = 120f;

    private float timer = 0f;
    private bool sudahXRay = false;
    private Renderer[] semuaBagian;

    void Start()
    {
        // Cukup cari dan data semua Mesh Renderer yang ada di anak-anak objek sesajen
        semuaBagian = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Jalankan timer kalau belum berubah jadi X-Ray
        if (!sudahXRay)
        {
            timer += Time.deltaTime; // Waktu berjalan...

            // Kalau waktu tunggu habis, langsung aktifkan efeknya
            if (timer >= waktuTunggu)
            {
                AktifkanXRay();
            }
        }
    }

    void AktifkanXRay()
    {
        if (materialXRay != null && semuaBagian != null)
        {
            // Langsung timpa material semua bagian sesajen (nampan, bunga, tiang) jadi material X-Ray
            foreach (Renderer bagian in semuaBagian)
            {
                if (bagian != null)
                {
                    bagian.material = materialXRay;
                }
            }

            sudahXRay = true;
            Debug.Log("Waktu habis! Sesajen " + gameObject.name + " sekarang berubah ke Material X-Ray.");
        }
    }
}