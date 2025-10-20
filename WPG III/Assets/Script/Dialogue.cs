using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        StartTyping();
        isDialogueActive = true;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
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
                isDialogueActive = false;
                return false;
            }
        }
    }

    public bool IsActive()
    {
        return isDialogueActive;
    }
}
