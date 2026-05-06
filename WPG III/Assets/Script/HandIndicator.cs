using UnityEngine;
using UnityEngine.UI;

public class HandIndicator : MonoBehaviour
{
    [Header("References")]
    public Image handImage;
    public GameObject interactPrompt;
    public float pickupRange = 2.5f;
    public float sesajenPickupRange = 4f;

    [Header("Optional: Layer Filtering")]
    [Tooltip("Leave empty to check all layers, or set to ignore furniture/walls")]
    public LayerMask raycastLayers = -1;

    void Update()
    {
        CheckForPickable();
    }

    void CheckForPickable()
    {
        Pickup held = Pickup.GetCurrentlyHeld();
        PickupSesajen heldSesajen = PickupSesajen.GetCurrentlyHeld();

        bool holdingNoodle = held != null &&
            (held.CompareTag("RawNoodle") || held.gameObject.name.ToLower().Contains("mie kuning"));

        // Hide if holding sesajen
        if (heldSesajen != null)
        {
            handImage.enabled = false;
            SetInteractPrompt(false);
            return;
        }

        // Hide if holding something that isn't a noodle
        if (held != null && !holdingNoodle)
        {
            handImage.enabled = false;
            SetInteractPrompt(false);
            return;
        }

        float maxRange = Mathf.Max(pickupRange, sesajenPickupRange);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxRange, raycastLayers);

        bool foundPickupInRange = false;
        bool foundInteractInRange = false;

        foreach (RaycastHit hit in hits)
        {
            float actualDistance = Vector3.Distance(transform.position, hit.point);

            // Only check pickable objects when not holding anything
            if (held == null && heldSesajen == null)
            {
                Pickup pickupComponent = hit.collider.GetComponent<Pickup>();
                if (pickupComponent != null && actualDistance <= pickupRange)
                {
                    foundPickupInRange = true;
                    break;
                }

                PickupSesajen sesajenComponent = hit.collider.GetComponent<PickupSesajen>();
                if (sesajenComponent != null && actualDistance <= sesajenPickupRange)
                {
                    foundPickupInRange = true;
                    break;
                }

                TutorialSesajen tutorialSesajen = hit.collider.GetComponentInParent<TutorialSesajen>();
                if (tutorialSesajen != null && actualDistance <= sesajenPickupRange)
                {
                    foundPickupInRange = true;
                    break;
                }
            }

            // Cooking pot checks
            TutorialNoodleCooking tutorialPot = hit.collider.GetComponent<TutorialNoodleCooking>();
            if (tutorialPot != null && actualDistance <= pickupRange)
            {
                bool potReadyToServe = tutorialPot.isCooked && !tutorialPot.isCooking;
                if (holdingNoodle || potReadyToServe)
                {
                    foundPickupInRange = true;
                    foundInteractInRange = true;
                    break;
                }
            }

            NoodleCooking noodleCooking = hit.collider.GetComponent<NoodleCooking>();
            if (noodleCooking != null && actualDistance <= pickupRange)
            {
                bool potReadyToServe = noodleCooking.isCooked && !noodleCooking.isCooking;
                if (holdingNoodle || potReadyToServe)
                {
                    foundPickupInRange = true;
                    foundInteractInRange = true;
                    break;
                }
            }
        }

        handImage.enabled = foundPickupInRange;
        SetInteractPrompt(foundInteractInRange);
    }

    private void SetInteractPrompt(bool visible)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(visible);
    }
}