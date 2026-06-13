using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCrashUI : MonoBehaviour
{

    void Start()
    {
        // Pastikan kursor muncul dan bebas bergerak saat game over
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Sembunyikan kursor lagi saat kembali bermain
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
        // Pastikan kursor terlihat saat di menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Ganti dengan nama persis di Build Settings kamu
        SceneManager.LoadScene("Main Menu Scene");
    }
}
