using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // <-- WAJIB buat pindah scene

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] stage1Lines;
    public string[] stage2Lines;
    public float textSpeed = 0.05f;

    private int index;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private bool inStage2 = false;
    private bool justSkipped = false; // Anti dobel skip

    public UnityEvent OnStage1Finished; // Event kamera pindah ke Bu Inah

    public void StartDialogue()
    {
        index = 0;
        inStage2 = false;
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

        string[] currentLines = inStage2 ? stage2Lines : stage1Lines;

        foreach (char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    public bool HandleInput()
    {
        string[] currentLines = inStage2 ? stage2Lines : stage1Lines;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textComponent.text = currentLines[index];
            isTyping = false;

            justSkipped = true;
            StartCoroutine(ResetSkipFlag());
            return true;
        }
        else
        {
            if (justSkipped) return true;

            if (index < currentLines.Length - 1)
            {
                index++;
                StartTyping();
                return true;
            }
            else
            {
                if (!inStage2) // Pindah ke stage 2
                {
                    inStage2 = true;
                    index = 0;
                    OnStage1Finished?.Invoke(); // Trigger kamera
                    StartTyping();
                    return true;
                }
                else // Stage 2 selesai
                {
                    Debug.Log("Dialog selesai, pindah Scene...");
                    SceneManager.LoadScene("GameScene"); // <---- GANTI SCENE DI SINI
                    return false;
                }
            }
        }
    }

    IEnumerator ResetSkipFlag()
    {
        yield return new WaitForSeconds(0.1f);
        justSkipped = false;
    }
}
