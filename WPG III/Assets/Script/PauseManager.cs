using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject dialogueBox; // TAMBAHAN

    public static bool isGamePaused = false;

    void Start()
    {
        // Pastikan pause menu tersembunyi
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        // SEMBUNYIKAN DIALOG SAAT PAUSE
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        Time.timeScale = 0f;
        isGamePaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioListener.pause = true;

        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        // TAMPILKAN LAGI DIALOG
        if (dialogueBox != null)
            dialogueBox.SetActive(true);

        Time.timeScale = 1f;
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioListener.pause = false;

        Debug.Log("Game Resumed");
    }

    public void Restart()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Main Menu Scene");
    }
}