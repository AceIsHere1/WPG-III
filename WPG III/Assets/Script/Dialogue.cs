using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        StartTyping();
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    public bool HandleInput()
    {
        if (isTyping)
        {
            // Skip → langsung tampil semua teks
            StopCoroutine(typingCoroutine);
            textComponent.text = lines[index];
            isTyping = false;
            return true; // masih di baris yang sama
        }
        else
        {
            // Sudah full → cek apakah masih ada line berikutnya
            if (index < lines.Length - 1)
            {
                index++;
                StartTyping();
                return true; // masih ada lanjut
            }
            else
            {
                gameObject.SetActive(false); // habis → sembunyikan
                return false; // sudah selesai
            }
        }
    }
}
