using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneUI : MonoBehaviour
{
    [SerializeField] private string nextNightScene = "Night2";
    [SerializeField] private string restartScene = "GameScene";
    [SerializeField] private string mainMenuScene = "Main Menu Scene";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnNextNight()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(nextNightScene);
    }

    public void OnRestart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(restartScene);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
