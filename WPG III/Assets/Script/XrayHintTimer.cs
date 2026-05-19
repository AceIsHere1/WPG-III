using UnityEngine;

public class XRayHintTimer : MonoBehaviour
{
    [Header("Pengaturan Material")]
    [Tooltip("Masukkan material asli/awal dari objek ini")]
    public Material materialBiasa;

    [Tooltip("Masukkan material XRay buatanmu yang warna merah tadi")]
    public Material materialXRay;

    [Header("Pengaturan Waktu")]
    [Tooltip("Waktu dalam detik sebelum XRay menyala (120 = 2 menit)")]
    public float waktuTunggu = 120f;

    private float timer = 0f;
    private MeshRenderer meshRenderer;
    private bool sudahXRay = false;

    void Start()
    {
        // Ambil komponen MeshRenderer dari objek tempat script ini dipasang
        meshRenderer = GetComponent<MeshRenderer>();

        // Pastikan saat mulai, material yang dipakai adalah material biasa
        if (meshRenderer != null && materialBiasa != null)
        {
            meshRenderer.material = materialBiasa;
        }
        else
        {
            Debug.LogWarning("Material Biasa belum dimasukkan di Inspector!");
        }
    }

    void Update()
    {
        // Kalau belum X-Ray, jalankan timer
        if (!sudahXRay)
        {
            timer += Time.deltaTime; // Waktu terus berjalan

            // Kalau waktu sudah mencapai batas (misal 120 detik)
            if (timer >= waktuTunggu)
            {
                AktifkanXRay();
            }
        }
    }

    void AktifkanXRay()
    {
        if (meshRenderer != null && materialXRay != null)
        {
            // Ganti materialnya jadi X-Ray (tembus pandang)
            meshRenderer.material = materialXRay;
            sudahXRay = true;
            Debug.Log("Hint X-Ray dinyalakan untuk: " + gameObject.name);
        }
    }
}