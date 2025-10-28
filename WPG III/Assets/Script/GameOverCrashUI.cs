using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCrashUI : MonoBehaviour
{
    [Header("Nama Scene Gameplay")]
    [Tooltip("Isi dengan nama scene utama untuk restart, misalnya 'GameScene'")]
    public string gameSceneName = "GameScene";

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
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}
