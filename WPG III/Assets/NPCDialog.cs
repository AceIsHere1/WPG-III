using System.Collections;
using UnityEngine;
using TMPro;

public class NpcDialog : MonoBehaviour
{
    [Header("UI Settings (global Canvas di scene)")]
    public TextMeshProUGUI dialogText;  // teks dialog di UI bawah layar
    public Canvas dialogCanvas;         // canvas tempat dialog muncul

    [Header("Durasi tampil dialog (detik)")]
    public float displayTime = 2f;

    private Coroutine dialogCoroutine;

    void Start()
    {
        if (dialogCanvas == null)
        {
            GameObject taggedCanvas = GameObject.FindGameObjectWithTag("DialogUI");
            if (taggedCanvas != null)
                dialogCanvas = taggedCanvas.GetComponent<Canvas>();
        }

        if (dialogText == null && dialogCanvas != null)
        {
            dialogText = dialogCanvas.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
        }

        if (dialogText != null)
            dialogText.text = "";
    }


    public void ShowDialog(string message)
    {
        if (dialogCoroutine != null)
            StopCoroutine(dialogCoroutine);

        dialogCoroutine = StartCoroutine(Display(message));
    }

    private IEnumerator Display(string msg)
    {
        if (dialogCanvas != null)
            dialogCanvas.enabled = true;

        if (dialogText != null)
            dialogText.text = msg;

        yield return new WaitForSeconds(displayTime);

        if (dialogText != null)
            dialogText.text = "";
    }
}
