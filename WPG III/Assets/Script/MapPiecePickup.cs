using UnityEngine;

public class MapPiecePickup : MonoBehaviour
{
    [Tooltip("The unique ID of this map piece (e.g., 0 for top-left, 1 for top-right)")]
    [SerializeField] private int pieceID;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure your Player prefab is tagged as "Player" in the Unity Editor
        if (other.CompareTag("Player"))
        {
            // Broadcast the collection event with this specific ID
            MapEvents.PieceCollected(pieceID);
            
            // Destroy the paper object in the 3D world
            Destroy(gameObject);
        }
    }
}