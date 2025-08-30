using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private bool isPickedUp = false;
    private Transform playerHand;

    void Update()
    {
        if (isPickedUp && playerHand != null)
        {
            // makanan akan terus nempel di tangan
            transform.position = playerHand.position;
            transform.rotation = playerHand.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E)) // tekan E untuk ambil
            {
                if (!isPickedUp)
                {
                    // cari objek tangan di player
                    playerHand = other.transform.Find("Hand");

                    if (playerHand != null)
                    {
                        isPickedUp = true;

                        // supaya makanan ikut tangan
                        transform.SetParent(playerHand);

                        // atur posisi relatif ke tangan
                        transform.localPosition = Vector3.zero;
                        transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        Debug.LogWarning("Player tidak punya objek bernama 'Hand'");
                    }
                }
            }
        }
    }
}
