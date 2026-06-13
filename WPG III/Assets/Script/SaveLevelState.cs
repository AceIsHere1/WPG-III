using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLevelState : MonoBehaviour
{
    void Start()
    {
        // 1. Ambil nama scene yang sedang aktif / sedang dimainkan sekarang
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 2. Simpan nama scene tersebut ke dalam laci memori bernama "LevelTerakhir"
        PlayerPrefs.SetString("LevelTerakhir", currentSceneName);
        PlayerPrefs.Save();

        Debug.Log("Game disimpan! Level saat ini: " + currentSceneName);
    }
}