using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The parent panel holding the entire map UI")]
    [SerializeField] private GameObject mapPanel;
    
    [Tooltip("Drag your map fragment UI Images here in order (0, 1, 2, etc.)")]
    [SerializeField] private Image[] mapPieceImages;

    private void OnEnable()
    {
        // Subscribe to the event when this script is active
        MapEvents.OnMapPieceCollected += UnlockMapPiece;
    }

    private void OnDisable()
    {
        // ALWAYS unsubscribe to prevent memory leaks when objects are destroyed
        MapEvents.OnMapPieceCollected -= UnlockMapPiece;
    }

    private void Start()
    {
        // Hide all map piece images at the start of the game
        foreach (Image piece in mapPieceImages)
        {
            piece.enabled = false; 
        }
        
        // Hide the main map panel initially
        mapPanel.SetActive(false);
    }

    private void UnlockMapPiece(int pieceID)
    {
        // Ensure the broadcasted ID exists in our array to prevent OutOfBounds errors
        if (pieceID >= 0 && pieceID < mapPieceImages.Length)
        {
            mapPieceImages[pieceID].enabled = true;
        }
    }

    private void Update()
    {
        // Toggle the map UI when the hotkey is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isCurrentlyActive = mapPanel.activeSelf;
            mapPanel.SetActive(!isCurrentlyActive);
            
            // Optional: You could pause the game here by setting Time.timeScale = 0f
        }
    }
}