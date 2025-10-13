using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
