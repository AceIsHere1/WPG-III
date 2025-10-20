using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public Camera playerCamera;
    public Camera buInahCamera;

    public void OnDialogueFinished()
    {
        StartCoroutine(SwitchToBuInah());
    }

    IEnumerator SwitchToBuInah()
    {
        yield return new WaitForSeconds(2f);

        playerCamera.enabled = false;
        buInahCamera.enabled = true;
    }
}
