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
    private GameObject dialogPanel;

    void Start()
    {
        if (dialogCanvas == null)
        {
            GameObject taggedCanvas = GameObject.FindGameObjectWithTag("DialogUI");
            if (taggedCanvas != null)
            {
                dialogCanvas = taggedCanvas.GetComponent<Canvas>();
                Debug.Log($"{name} berhasil menemukan Canvas: {dialogCanvas.name}");
            }
            else
            {
                Debug.LogWarning($"{name} tidak menemukan Canvas bertag 'DialogUI'");
            }
        }

        if (dialogText == null && dialogCanvas != null)
        {
            dialogText = dialogCanvas.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
            Debug.Log($"{name} berhasil menemukan TextMeshPro: {dialogText.name}");
        }

        if (dialogText != null)
            dialogPanel = dialogText.transform.parent.gameObject;

        if (dialogPanel != null)
            dialogPanel.SetActive(false);

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

        if (dialogPanel != null)
            dialogPanel.SetActive(true);

        if (dialogText != null)
            dialogText.text = msg;

        yield return new WaitForSeconds(displayTime);

        if (dialogText != null)
            dialogText.text = "";

        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }
}
