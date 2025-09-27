using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private Canvas bubbleCanvas;       // Canvas bubble
    [SerializeField] private TextMeshProUGUI chatText;  // Teks di dalam bubble
    [SerializeField] private float displayTime = 3f;    // Durasi tampil

    private float timer;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        // Hitung mundur buat auto hide
        if (bubbleCanvas.enabled)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Hide();
            }
        }
    }

    // Munculkan teks
    public void Show(string message, float duration = -1f)
    {
        chatText.text = message;
        bubbleCanvas.enabled = true;

        // Kalau ada custom duration dipakai, kalau nggak default
        timer = duration > 0 ? duration : displayTime;
    }

    // Sembunyikan bubble
    public void Hide()
    {
        bubbleCanvas.enabled = false;
    }
}
