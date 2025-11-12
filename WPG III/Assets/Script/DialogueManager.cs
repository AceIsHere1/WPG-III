using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueScript;
    public GameObject dialoguePanel;

    private bool isDialogueActive = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
                {
                    dialoguePanel.SetActive(false);
                    isDialogueActive = false;
                }
            }
        }
    }
}
