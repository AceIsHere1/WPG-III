using UnityEngine;

public class NpcInteract : MonoBehaviour
{
    [SerializeField] private ChatBubble chatBubble;
    [TextArea][SerializeField] private string[] dialogLines;
    private int currentLine = 0;

    public void Interact()
    {
        if (chatBubble == null)
        {
            Debug.LogError($"{name}: chatBubble belum diassign pada NpcInteract!", this);
            return;
        }

        if (dialogLines == null || dialogLines.Length == 0)
        {
            Debug.LogWarning($"{name}: dialogLines kosong -> tidak ada yang ditampilkan.", this);
            return;
        }

        if (currentLine < dialogLines.Length)
        {
            chatBubble.Show(dialogLines[currentLine]);
            currentLine++;
        }
        else
        {
            chatBubble.Hide();
            currentLine = 0;
        }
    }
}
