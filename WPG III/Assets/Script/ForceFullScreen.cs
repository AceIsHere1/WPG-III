using System.Collections;
using UnityEngine;

public class ForceFullscreen : MonoBehaviour
{
    void Awake()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
    }

    IEnumerator Start()
    {
        yield return null;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}