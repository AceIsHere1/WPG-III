using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        // Pastikan kursor muncul dan bebas bergerak
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Sembunyikan kursor lagi saat kembali bermain (opsional)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Pindah ke scene gameplay
        // BACA MEMORI: Ambil nama level dari laci "LevelTerakhir". 
        // Jika laci kosong (misal baru pertama main), defaultnya pindah ke "GameScene".
        string levelTujuan = PlayerPrefs.GetString("LevelTerakhir", "GameScene");

        // Pindah ke scene hasil catatan memori
        SceneManager.LoadScene(levelTujuan);
    }

    public void ReturnToMainMenu()
    {
        // Pastikan kursor aktif lagi saat kembali ke menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pindah ke scene Main Menu (ubah namanya sesuai scene kamu)
        SceneManager.LoadScene("Main Menu Scene");
    }
}
