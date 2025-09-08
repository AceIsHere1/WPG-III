using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform pickupPoint;
    private Transform originalPoint;
    private bool pickedUp;

    [Header("Sound Settings")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    private Rigidbody rb;

    void Start()
    {
        originalPoint = pickupPoint;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        // setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // kalau lagi dipegang, object selalu ikut pickupPoint
        if (pickedUp)
        {
            rb.MovePosition(pickupPoint.position);
            rb.MoveRotation(pickupPoint.rotation);
        }
    }

    void OnMouseDown()
    {
        pickedUp = true;

        // disable physics biar ga jatuh
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.freezeRotation = true;

        GetComponent<BoxCollider>().enabled = false;

        // mainkan suara pickup
        if (pickupSound != null)
            audioSource.PlayOneShot(pickupSound);
    }

    void OnMouseUp()
    {
        pickedUp = false;

        // aktifkan physics lagi
        rb.useGravity = true;
        rb.freezeRotation = false;

        GetComponent<BoxCollider>().enabled = true;

        // mainkan suara drop
        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
    }
}
