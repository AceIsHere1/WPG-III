using UnityEngine;
using UnityEngine.UI;

public class HandIndicator : MonoBehaviour
{
    [Header("References")]
    public Image handImage;
    public float pickupRange = 2.5f; // Range for regular Pickup objects
    public float sesajenPickupRange = 4f; // Range for PickupSesajen objects
    
    [Header("Optional: Layer Filtering")]
    [Tooltip("Leave empty to check all layers, or set to ignore furniture/walls")]
    public LayerMask raycastLayers = -1; // -1 means all layers

    void Update()
    {
        CheckForPickable();
    }

    void CheckForPickable()
    {
        // Hide indicator if already holding something
        if (Pickup.GetCurrentlyHeld() != null || PickupSesajen.GetCurrentlyHeld() != null)
        {
            handImage.enabled = false;
            return;
        }

        // Use the longer range to check all objects (sesajen range is longer)
        float maxRange = Mathf.Max(pickupRange, sesajenPickupRange);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxRange, raycastLayers);
        
        bool foundPickupInRange = false;
        
        foreach (RaycastHit hit in hits)
        {
            float actualDistance = Vector3.Distance(transform.position, hit.point);
            
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

            TutorialNoodleCooking cookingPot = hit.collider.GetComponent<TutorialNoodleCooking>();
            if (cookingPot != null && actualDistance <= pickupRange)
            {
                Pickup held = Pickup.GetCurrentlyHeld();
                bool holdingNoodle = held != null && 
                    (held.CompareTag("RawNoodle") || held.gameObject.name.ToLower().Contains("mie kuning"));
                
                // Also show hand if pot is cooked and ready to serve
                bool potReadyToServe = cookingPot.isCooked && !cookingPot.isCooking;

                if (holdingNoodle || potReadyToServe)
                {
                    foundPickupInRange = true;
                    break;
                }
            }
        }
        
        handImage.enabled = foundPickupInRange;
    }
}