using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Kalau yang masuk area punya komponen Rigidbody dan tag "Sesajen"
        if (other.CompareTag("Sesajen"))
        {
            // Cek kalau player menekan tombol R
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Buang (hapus) sesajen
                Destroy(other.gameObject);
                Debug.Log("Sesajen dibuang ke tempat sampah!");
            }
        }
    }
}
