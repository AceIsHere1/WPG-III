using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    public GameObject howToPlayPanel;
    private bool isPanelActive = false;

    void Update()
    {
        // Cek input ESC
        if (Input.GetKeyDown(KeyCode.Escape) && isPanelActive)
        {
            CloseHowToPlay();
        }
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        isPanelActive = true;
        Time.timeScale = 0f; // opsional, pause game kalau mau
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        isPanelActive = false;
        Time.timeScale = 1f;
    }
}
