using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;
    public Transform[] destinations; // isi manual di inspector
    public Transform orderPoint;

    private void Awake()
    {
        Instance = this;
    }
}