using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public CanvasGroup dialogueUI;

    public static bool isGamePaused = false;

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
        pauseMenu.SetActive(true);

        if (dialogueUI != null)
        {
            dialogueUI.alpha = 0f;            // sembunyikan dialog
            dialogueUI.interactable = false;
            dialogueUI.blocksRaycasts = false;
        }

        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        if (dialogueUI != null)
        {
            dialogueUI.alpha = 1f;            // munculkan lagi dialog
            dialogueUI.interactable = true;
            dialogueUI.blocksRaycasts = true;
        }

        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene("Main Menu Scene");
    }

    public void QuitGame()
    {
        Resume();
        Application.Quit();
        Debug.Log("Game Closed");
    }
}