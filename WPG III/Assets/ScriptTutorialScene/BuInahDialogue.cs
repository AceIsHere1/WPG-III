using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuInahDialogue : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;

    void Start()
    {
        if (nameText != null)
            nameText.text = ""; // kosongkan dulu agar bisa diisi sesuai pembicara

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Menampilkan dialog dengan nama pembicara dan teksnya.
    /// </summary>
    public IEnumerator Speak(string speakerName, string message)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // Ubah nama pembicara
        if (nameText != null)
            nameText.text = speakerName + ":";

        // Reset teks dialog
        if (dialogueText != null)
            dialogueText.text = "";

        // Efek mengetik huruf demi huruf
        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Tunggu sejenak sebelum lanjut dialog berikut
        yield return new WaitForSeconds(1.2f);
    }

    /// <summary>
    /// Sembunyikan panel dialog.
    /// </summary>
    public void Hide()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}
