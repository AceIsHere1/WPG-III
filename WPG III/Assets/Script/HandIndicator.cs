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
            // Check actual distance to hit point
            float actualDistance = Vector3.Distance(transform.position, hit.point);
            
            // Check if this is a regular Pickup component
            Pickup pickupComponent = hit.collider.GetComponent<Pickup>();
            if (pickupComponent != null && actualDistance <= pickupRange)
            {
                foundPickupInRange = true;
                break;
            }
            
            // Check if this is a PickupSesajen component (with longer range)
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
        
        handImage.enabled = foundPickupInRange;
    }
}