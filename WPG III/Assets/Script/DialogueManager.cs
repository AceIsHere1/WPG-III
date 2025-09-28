using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueScript;
    private bool isDialogueActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive)
            {
                // Aktifkan UI dialogue box
                dialogueScript.gameObject.SetActive(true);
                dialogueScript.StartDialogue();
                isDialogueActive = true;
            }
            else
            {
                // Tutup dialogue box kalau sudah aktif
                dialogueScript.gameObject.SetActive(false);
                isDialogueActive = false;
            }
        }
    }
}
