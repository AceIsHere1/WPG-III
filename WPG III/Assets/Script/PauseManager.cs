using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public static bool isGamePaused = false;

    void Start()
    {
        // Make sure pause menu starts hidden
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenu not assigned!");
        }
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

        Time.timeScale = 0f;
        isGamePaused = true;
        
        // Show and unlock cursor for menu interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Mute all audio
        AudioListener.pause = true;
        
        Debug.Log("Game Paused - Cursor unlocked, Audio muted");
    }

    public void Resume()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isGamePaused = false;
        
        // Lock and hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Unmute audio
        AudioListener.pause = false;
        
        Debug.Log("Game Resumed - Cursor locked, Audio unmuted");
    }

    public void Restart()
    {
        Debug.Log("Restart button clicked!");
        
        // Important: Unmute audio before restarting
        AudioListener.pause = false;
        
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu button clicked!");
        
        // Important: Unmute audio before changing scenes
        AudioListener.pause = false;
        
        Resume();
        SceneManager.LoadScene("Main Menu Scene");
    }
}