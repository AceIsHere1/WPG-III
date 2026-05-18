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
            }
        }
    }

    // Dipanggil saat player tekan 'E'
    public void Refill()
    {
        power = 100f;
        SetGenderuwoState(false);
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
}