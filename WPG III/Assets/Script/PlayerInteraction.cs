using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    private GameObject heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Jika yang diinteract adalah panci
            NoodleCooking cooker = hit.collider.GetComponent<NoodleCooking>();
            if (cooker != null && heldItem != null && heldItem.CompareTag("RawNoodle"))
            {
                Destroy(heldItem);   // buang mie mentah
                cooker.StartCooking();
                heldItem = null;
            }
            // Jika yang diinteract adalah bungkus mie
            else if (hit.collider.CompareTag("NoodlePack"))
            {
                Destroy(hit.collider.gameObject); // hancurkan bungkus
                heldItem = Instantiate(Resources.Load<GameObject>("RawNoodle")); // spawn mie kotak kuning di tangan
                heldItem.tag = "RawNoodle";
            }
        }
    }
}