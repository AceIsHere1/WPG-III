using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcInteract : MonoBehaviour
{
    [SerializeField] private ChatBubble chatBubble;
    [SerializeField] private string[] dialogLines; // teks dialog NPC
    private int currentLine = 0;

    public void Interact()
    {
        if (dialogLines.Length == 0) return;

        if (currentLine < dialogLines.Length)
        {
            chatBubble.Show(dialogLines[currentLine]);
            currentLine++;
        }
        else
        {
            chatBubble.Hide();
            currentLine = 0; // reset supaya bisa ulang lagi
        }
    }
}

