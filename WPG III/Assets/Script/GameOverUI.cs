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

        // Ganti dengan nama scene gameplay kamu
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}
