using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    [SerializeField] private string winSceneName = "WinScene"; // nama scene kemenangan di Build Settings

    private void OnEnable()
    {
        GameEvents.OnGameWon += HandleGameWon;
    }

    private void OnDisable()
    {
        GameEvents.OnGameWon -= HandleGameWon;
    }

    private void HandleGameWon()
    {
        Debug.Log("Game menang! Memuat scene kemenangan...");
        SceneManager.LoadScene(winSceneName);
    }
}
