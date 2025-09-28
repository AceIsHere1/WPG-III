using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Dialogue dialogueScript;

    private bool isDialogueActive = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive)
            {
                dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
                isDialogueActive = true;
            }
            else
            {
                bool stillGoing = dialogueScript.HandleInput();
                if (!stillGoing)
                    isDialogueActive = false;
            }
        }
    }
}
