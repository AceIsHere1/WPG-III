using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private CanvasGroup dialogueUI;

    public static bool isGamePaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Jika sedang membuka Controls Panel
            if (controlsPanel != null && controlsPanel.activeSelf)
            {
                CloseControls();
                return;
            }

            // Jika game sedang pause
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

        // Pastikan controls panel tidak aktif saat pause
        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        // sembunyikan dialog
        if (dialogueUI != null)
            dialogueUI.alpha = 0f;

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

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        // tampilkan lagi dialog
        if (dialogueUI != null)
            dialogueUI.alpha = 1f;

        Time.timeScale = 1f;
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioListener.pause = false;

        Debug.Log("Game Resumed");
    }

    // =========================
    // CONTROLS PANEL
    // =========================

    public void OpenControls()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
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