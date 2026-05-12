using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;

    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false; // starts off
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            flashlight.enabled = !flashlight.enabled;
    }
}