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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive)
            {
                // Aktifkan UI dialogue box
                dialogueScript.gameObject.SetActive(true);
                dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
                isDialogueActive = true;
            }
            else
            {
                // Cek dulu apakah lanjut/minta next line
                bool stillGoing = dialogueScript.HandleInput();

                // Kalau sudah selesai, matikan UI
                if (!stillGoing)
                {
                    dialogueScript.gameObject.SetActive(false);
                    dialoguePanel.SetActive(false);
                    isDialogueActive = false;
                }
            }
        }
    }
}


