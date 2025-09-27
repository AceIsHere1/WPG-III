using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubble;        // root panel image (child)
    [SerializeField] private TextMeshProUGUI chatText; // TMP text inside
    [SerializeField] private float displayTime = 3f;

    private float timer;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        if (bubble == null) Debug.LogError("ChatBubble: bubble GameObject belum di-assign!", this);
        if (chatText == null) Debug.LogError("ChatBubble: chatText (TMP) belum di-assign!", this);
        Hide();
    }

    private void Update()
    {
        // billboard: selalu menghadap kamera
        if (mainCam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        }

        if (bubble != null && bubble.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) Hide();
        }
    }

    public void Show(string message, float duration = -1f)
    {
        if (chatText == null || bubble == null) return;
        chatText.text = message;
        bubble.SetActive(true);
        timer = (duration > 0f) ? duration : displayTime;
    }

    public void Hide()
    {
        if (bubble != null) bubble.SetActive(false);
    }
}
