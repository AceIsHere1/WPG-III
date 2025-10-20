using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;
    public CutsceneController cutsceneController; // 🔹 referensi ke cutscene

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
            StopCoroutine(typingCoroutine);
            textComponent.text = lines[index];
            isTyping = false;
            return true;
        }
        else
        {
            if (index < lines.Length - 1)
            {
                index++;
                StartTyping();
                return true;
            }
            else
            {
                gameObject.SetActive(false);

                // 🔹 Panggil cutscene setelah dialog terakhir selesai
                if (cutsceneController != null)
                    cutsceneController.TriggerCutscene();

                return false;
            }
        }
    }
}
