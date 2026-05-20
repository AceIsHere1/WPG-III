using UnityEngine;
using TMPro; // Wajib ditambahkan untuk memanggil UI TextMeshPro

public class GeneratorController : MonoBehaviour
{
    [Header("Status Daya")]
    public float power = 100f;
    public float depletionRate = 5f; // Habis dalam 20 detik (100 / 5)

    [Header("Referensi Objek")]
    public GameObject genderuwo;
    public TextMeshProUGUI powerTextUI; // Masukkan PowerTextUI ke sini

    [Header("Pengaturan Lampu Lingkungan")]
    [Tooltip("Masukkan semua objek lampu yang ingin dimatikan ke sini")]
    public GameObject[] environmentLights; // Array untuk menyimpan banyak lampu sekaligus

    private bool isGenderuwoActive = false;

    void Start()
    {
        // Kondisi awal game dijamin aman
        power = 100f;
        isGenderuwoActive = false;

        if (genderuwo != null)
        {
            genderuwo.SetActive(false); // Matikan wujud genderuwo di awal
        }

        // Pastikan semua lampu menyala saat game baru dimulai
        SetLightsState(true);
    }

    void Update()
    {
        // Kurangi daya selama game berjalan
        if (power > 0)
        {
            power -= depletionRate * Time.deltaTime;

            // Update UI Teks. Mathf.CeilToInt biar angkanya bulat, gak ada koma pusing
            if (powerTextUI != null)
            {
                powerTextUI.text = "Daya: " + Mathf.CeilToInt(power).ToString() + "%";
            }

            // Memicu genderuwo saat daya habis
            if (power <= 0)
            {
                power = 0;
                if (powerTextUI != null) powerTextUI.text = "Daya: 0% (BAHAYA!)";
                SetGenderuwoState(true);

                // Panggil fungsi untuk mematikan lampu
                SetLightsState(false);
            }
        }
    }

    // Dipanggil saat player tekan 'E'
    public void Refill()
    {
        power = 100f;
        SetGenderuwoState(false);

        // Panggil fungsi untuk menyalakan lampu kembali
        SetLightsState(true);

        Debug.Log("Generator berhasil diisi!");
    }

    // Fungsi rapi untuk memunculkan/menghilangkan genderuwo
    void SetGenderuwoState(bool state)
    {
        if (isGenderuwoActive != state)
        {
            isGenderuwoActive = state;
            if (genderuwo != null)
            {
                genderuwo.SetActive(isGenderuwoActive);
            }
        }
    }

    // --- FUNGSI BARU UNTUK LAMPU ---
    void SetLightsState(bool isOn)
    {
        // Mengecek satu per satu objek lampu yang ada di dalam daftar array
        foreach (GameObject lightObj in environmentLights)
        {
            if (lightObj != null)
            {
                // SetActive(true) untuk nyala, SetActive(false) untuk mati
                lightObj.SetActive(isOn);
            }
        }
    }
}