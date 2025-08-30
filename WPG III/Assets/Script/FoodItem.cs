using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private bool isPlayerNearby = false;
    private bool isHeld = false;
    private Transform playerHand;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerHand = other.transform.Find("HandPoint");
            Debug.Log("Dekat makanan - Tekan E untuk mengambil.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerHand = null;
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isHeld && Input.GetKeyDown(interactKey))
        {
            if (playerHand != null)
            {
                // tempel ke tangan
                transform.SetParent(playerHand);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                isHeld = true;
                Debug.Log("Makanan dipegang player!");
            }
        }
    }
}
