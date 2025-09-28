using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float holdDistance = 2f;
    public float smoothSpeed = 10f;
    public float pickupRange = 1f;

    [Header("Internal")]
    [SerializeField] private bool pickedUp;
    private Rigidbody rb;

    [Header("Sound Settings")]
    public AudioClip pickupSound;
    public AudioClip unwrapSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    [Header("Noodle Cooking Settings")]
    public GameObject rawNoodlePrefab;
    public bool isNoodlePack = false;

    private static Pickup currentlyHeld;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (pickedUp && currentlyHeld == this)
        {
            if (playerCamera != null)
            {
                Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
                if (rb != null)
                {
                    rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * smoothSpeed));
                    rb.MoveRotation(Quaternion.Lerp(rb.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed));
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, playerCamera.rotation, Time.deltaTime * smoothSpeed);
                }
            }

            if (Input.GetMouseButtonDown(1)) Drop();
            if (isNoodlePack && Input.GetKeyDown(KeyCode.E)) UnwrapNoodle();
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) TryPickup();
        }
    }

    private void TryPickup()
    {
        if (currentlyHeld != null) return;
        if (playerCamera == null) playerCamera = Camera.main != null ? Camera.main.transform : null;
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.transform == transform)
            {
                pickedUp = true;
                currentlyHeld = this;
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                if (pickupSound != null && audioSource != null) audioSource.PlayOneShot(pickupSound);
            }
        }
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;
        pickedUp = false;
        currentlyHeld = null;
        if (rb != null) rb.useGravity = true;
        if (dropSound != null && audioSource != null) audioSource.PlayOneShot(dropSound);
    }

    private void UnwrapNoodle()
    {
        if (rawNoodlePrefab == null) return;

        // Spawn mie kotak kuning di posisi bungkus
        GameObject noodle = Instantiate(rawNoodlePrefab, transform.position, transform.rotation);

        // Otomatis pickup mie kotak kuning
        Pickup noodlePickup = noodle.GetComponent<Pickup>();
        if (noodlePickup != null) noodlePickup.ForcePickup();

        // Mainkan sound juga saat buka bungkus
        if (unwrapSound != null && audioSource != null)
            audioSource.PlayOneShot(unwrapSound);

        // Hancurkan bungkus mie
        Destroy(gameObject, unwrapSound.length);
    }

    // PUBLIC API untuk skrip lain
    public void ForcePickup()
    {
        if (currentlyHeld != null) currentlyHeld.ForceDrop();
        pickedUp = true;
        currentlyHeld = this;
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (pickupSound != null && audioSource != null) audioSource.PlayOneShot(pickupSound);
    }

    public void ForceDrop()
    {
        if (currentlyHeld == this) { pickedUp = false; currentlyHeld = null; }
        if (rb != null) rb.useGravity = true;
        if (dropSound != null && audioSource != null) audioSource.PlayOneShot(dropSound);
    }

    public static Pickup GetCurrentlyHeld()
    {
        return currentlyHeld;
    }

    public bool IsPickedUp()
    {
        return pickedUp;
    }
}
