using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class ini membuat kita bisa mengelompokkan Gambar Komik dan Kotak Penutupnya di Inspector
[System.Serializable]
public class HalamanKomik
{
    [Tooltip("Masukkan gambar halaman komik utuh")]
    public Sprite gambarHalaman;

    [Tooltip("Masukkan kotak putih penutup secara berurutan (misal: penutup panel 2, penutup panel 3)")]
    public List<Image> daftarPenutup;
}

public class ComicCinematic : MonoBehaviour
{
    [Header("Pengaturan UI")]
    public Image comicDisplay;
    public float fadeSpeed = 3f; // Dipercepat sedikit agar tidak bosan menunggu

    [Header("Daftar Halaman & Panel")]
    public List<HalamanKomik> daftarHalaman;

    private int indexHalaman = 0;
    private int indexPenutup = 0;
    private bool isAnimating = false; // Mencegah pemain spam klik

    void Start()
    {
        if (daftarHalaman.Count > 0)
        {
            SiapkanHalaman(indexHalaman);
        }
    }

    void Update()
    {
        // Deteksi input klik kiri atau Spasi
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isAnimating)
        {
            ProsesLanjut();
        }
    }

    // --- BAGIAN INI YANG DIUBAH ---
    void SiapkanHalaman(int index)
    {
        // Pasang gambar komik utama
        comicDisplay.sprite = daftarHalaman[index].gambarHalaman;
        comicDisplay.color = Color.white;

        // 1. MATIKAN dulu SEMUA penutup dari semua halaman agar tidak tumpang tindih
        for (int h = 0; h < daftarHalaman.Count; h++)
        {
            foreach (Image p in daftarHalaman[h].daftarPenutup)
            {
                if (p != null) p.gameObject.SetActive(false);
            }
        }

        // 2. NYALAKAN hanya penutup milik halaman yang sedang aktif saat ini
        foreach (Image penutup in daftarHalaman[index].daftarPenutup)
        {
            if (penutup != null)
            {
                penutup.gameObject.SetActive(true); // Munculkan kotaknya
                penutup.color = Color.white;        // Pastikan warnanya putih
            }
        }

        // Reset hitungan penutup ke 0 setiap kali ganti halaman
        indexPenutup = 0;
    }
    // ------------------------------

    void ProsesLanjut()
    {
        HalamanKomik halamanAktif = daftarHalaman[indexHalaman];

        // SKENARIO 1: Masih ada kotak penutup yang harus dihilangkan di halaman ini
        if (indexPenutup < halamanAktif.daftarPenutup.Count)
        {
            StartCoroutine(FadeOutPenutup(halamanAktif.daftarPenutup[indexPenutup]));
            indexPenutup++;
        }
        // SKENARIO 2: Semua penutup sudah hilang, ganti ke halaman komik berikutnya
        else if (indexHalaman < daftarHalaman.Count - 1)
        {
            indexHalaman++;
            StartCoroutine(GantiHalaman(daftarHalaman[indexHalaman]));
        }
        // SKENARIO 3: Halaman terakhir sudah selesai
        else
        {
            Debug.Log("Komik Selesai! Saatnya munculkan tombol Main Menu.");
            // Taruh kodemu untuk memunculkan UI ending di sini
        }
    }

    IEnumerator FadeOutPenutup(Image penutup)
    {
        isAnimating = true;
        for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * fadeSpeed)
        {
            penutup.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        penutup.color = new Color(1, 1, 1, 0); // Kunci di 0 agar benar-benar tembus pandang
        isAnimating = false;
    }

    IEnumerator GantiHalaman(HalamanKomik halamanBaru)
    {
        isAnimating = true;

        // 1. Fade out gambar halaman lama
        for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * fadeSpeed)
        {
            comicDisplay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 2. Ganti sprite dan reset kotak penutup untuk halaman baru
        SiapkanHalaman(indexHalaman);

        // 3. Fade in gambar halaman baru
        for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * fadeSpeed)
        {
            comicDisplay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        comicDisplay.color = Color.white;
        isAnimating = false;
    }
}