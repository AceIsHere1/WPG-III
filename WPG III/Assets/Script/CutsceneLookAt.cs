using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Transform buInah;
    public MonoBehaviour playerController;

    [Header("Settings")]
    public float lookSpeed = 2f;
    public float delayBeforeLook = 2f; // waktu merenung
    public float duration = 5f;        // berapa lama kamera menghadap bu Inah

    private bool isCutsceneActive = false;
    private float timer = 0f;

    void Update()
    {
        if (!isCutsceneActive) return;

        timer += Time.deltaTime;

        // Setelah delay merenung, mulai putar kamera
        if (timer >= delayBeforeLook && timer <= delayBeforeLook + duration)
        {
            Vector3 direction = (buInah.position - playerCamera.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * lookSpeed);
        }

        // Setelah selesai durasi, kembalikan kontrol player
        if (timer > delayBeforeLook + duration)
        {
            EndCutscene();
        }
    }

    public void TriggerCutscene()
    {
        if (playerController != null)
            playerController.enabled = false; // matikan kontrol player

        timer = 0f;
        isCutsceneActive = true;
    }

    private void EndCutscene()
    {
        if (playerController != null)
            playerController.enabled = true; // nyalakan kontrol player lagi
        isCutsceneActive = false;
    }
}
