using UnityEngine;

public class MapPositionTranslator : MonoBehaviour
{
    [Header("World Space Configuration")]
    [Tooltip("Drag your 3D Player transform here.")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("The bottom-left corner limits of your walkable 3D level map (X, Z).")]
    [SerializeField] private Vector2 worldMinBounds;

    [Tooltip("The top-right corner limits of your walkable 3D level map (X, Z).")]
    [SerializeField] private Vector2 worldMaxBounds;

    [Header("UI Configuration")]
    [Tooltip("Drag the PlayerMarker UI Image here.")]
    [SerializeField] private RectTransform playerUIMarker;

    private RectTransform mapRect;

    private void Awake()
    {
        // Cache the RectTransform of the MapPanel this script is attached to
        mapRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Only run calculations if the map panel is active and references are set
        if (playerTransform == null || playerUIMarker == null) return;

        UpdateMarkerPosition();
    }

    private void UpdateMarkerPosition()
    {
        // 1. Grab the player's 3D position
        Vector3 playerWorldPos = playerTransform.position;

        // 2. Convert world coordinates to a percentage (0.0 to 1.0) relative to your boundaries
        float normalizedX = Mathf.InverseLerp(worldMinBounds.x, worldMaxBounds.x, playerWorldPos.x);
        // Note: We use playerWorldPos.z because 'forward' in 3D world space maps to 'up' on a 2D map
        float normalizedY = Mathf.InverseLerp(worldMinBounds.y, worldMaxBounds.y, playerWorldPos.z);

        // 3. Scale that percentage to match the actual width and height pixel boundaries of your UI map
        float uiX = Mathf.Lerp(mapRect.rect.xMin, mapRect.rect.xMax, normalizedX);
        float uiY = Mathf.Lerp(mapRect.rect.yMin, mapRect.rect.yMax, normalizedY);

        // 4. Update the UI element's position
        playerUIMarker.anchoredPosition = new Vector2(uiX, uiY);
    }
}