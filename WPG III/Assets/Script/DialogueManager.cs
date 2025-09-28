using UnityEngine;
using UnityEngine.SceneManagement; // buat pindah scene

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Dialogue dialogueScript;

    private bool isDialogueActive = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false); // dialog hidden saat awal
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive)
            {
                // Mulai dialog
                dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
                isDialogueActive = true;
            }
            else
            {
                // Handle input dari Dialogue.cs (lanjut atau skip text)
                bool stillGoing = dialogueScript.HandleInput();

                if (!stillGoing)
                {
                    // Dialog selesai → sembunyikan panel
                    dialoguePanel.SetActive(false);
                    isDialogueActive = false;

                    // Pindah ke GameScene
                    SceneManager.LoadScene("GameScene");
                }
            }
        }
    }
}
