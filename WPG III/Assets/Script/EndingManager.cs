using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    // Fungsi untuk kembali ke Main Menu
    public void KeMainMenu()
    {
        // Pastikan nama ini sama persis dengan nama file scene menu utama kamu
        SceneManager.LoadScene("Main Menu Scene");
    }

    // Fungsi untuk keluar dari game
    public void KeluarGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}