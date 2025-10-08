using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Kalau objek yang masuk punya tag "Sesajen"
        if (other.CompareTag("Sesajen"))
        {
            // Langsung buang sesajen begitu tombol R ditekan (tanpa nunggu frame berikut)
            if (Input.GetKey(KeyCode.R))
            {
                Destroy(other.gameObject);
                Debug.Log("Sesajen langsung dibuang ke tempat sampah!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Tambahan biar tetap bisa dibuang kalau belum sempat ditekan pas baru masuk
        if (other.CompareTag("Sesajen") && Input.GetKeyDown(KeyCode.R))
        {
            Destroy(other.gameObject);
            Debug.Log("Sesajen dibuang ke tempat sampah!");
        }
    }
}
