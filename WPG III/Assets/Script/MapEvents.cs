using System;

public static class MapEvents
{
    // The event other scripts will listen to
    public static event Action<int> OnMapPieceCollected;

    // The method to trigger the event
    public static void PieceCollected(int pieceID)
    {
        OnMapPieceCollected?.Invoke(pieceID);
    }
}