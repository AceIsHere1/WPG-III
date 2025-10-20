using System.Collections;
using UnityEngine;

public class PrologueCutsceneController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerCamera;     // Kamera utama (yang nempel di player)
    public Transform buInah;           // Posisi Bu Inah
    public float lookSpeed = 2f;       // Kecepatan rotasi kamera
    public float lookDuration = 3f;    // Durasi rotasi menghadap Bu Inah
    public float delayBeforeLook = 2f; // Waktu setelah Minto merenung

    [Header("Dialogue")]
    public Dialogue buInahDialogue;    // Script dialogue Bu Inah

    private bool isLookingAtBuInah = false;
    private float timer = 0f;

    public void StartCameraLook()
    {
        StartCoroutine(LookAtBuInahRoutine());
    }

    private IEnumerator LookAtBuInahRoutine()
    {
        yield return new WaitForSeconds(delayBeforeLook); // waktu merenung

        isLookingAtBuInah = true;
        timer = 0f;

        while (timer < lookDuration)
        {
            timer += Time.deltaTime;
            Vector3 direction = (buInah.position - playerCamera.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * lookSpeed);
            yield return null;
        }

        isLookingAtBuInah = false;

        // Setelah kamera selesai menghadap, mulai dialog Bu Inah
        if (buInahDialogue != null)
            buInahDialogue.StartDialogue();
    }
}
