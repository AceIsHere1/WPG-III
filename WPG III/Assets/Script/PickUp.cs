using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float holdDistance = 2f;
    public float smoothSpeed = 20f;
    public float pickupRange = 2.5f;

    [Header("Internal")]
    [SerializeField] private bool pickedUp;
    private Rigidbody rb;

    [Header("Sound Settings")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float pickupSoundVolume = 1f;

    public AudioClip unwrapSound;
    [Range(0f, 1f)] public float unwrapSoundVolume = 1f;

    public AudioClip dropSound;
    [Range(0f, 1f)] public float dropSoundVolume = 1f;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    [Header("Noodle Cooking Settings")]
    public GameObject rawNoodlePrefab;
    public bool isNoodlePack = false;

    private static Pickup currentlyHeld;
    public Vector3 heldRotation = new Vector3(-1f, 187f, -88f);

    private int heldLayer;
    private int unheldLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;

        heldLayer = LayerMask.NameToLayer("HeldObject");
        unheldLayer = LayerMask.NameToLayer("UnheldObject");
    }

    void FixedUpdate()
    {
        if (pickedUp && currentlyHeld == this)
        {
            if (playerCamera != null && rb != null)
            {
                Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
                Vector3 delta = targetPos - rb.position;

                rb.velocity = delta * smoothSpeed;
                rb.angularVelocity = Vector3.zero;

                Quaternion targetRot = playerCamera.rotation * Quaternion.Euler(heldRotation);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * smoothSpeed));
            }
        }
    }

    void Update()
    {
        if (pickedUp && currentlyHeld == this)
        {
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

        RaycastHit[] hits = Physics.RaycastAll(playerCamera.position, playerCamera.forward, pickupRange);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == transform)
            {
                float actualDistance = Vector3.Distance(playerCamera.position, hit.point);

                if (actualDistance <= pickupRange)
                {
                    PerformPickup();
                    return;
                }
            }
        }
    }

    private void PerformPickup()
    {
        pickedUp = true;
        currentlyHeld = this;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        gameObject.layer = heldLayer;

        if (pickupSound != null && audioSource != null)
            audioSource.PlayOneShot(pickupSound, pickupSoundVolume);
    }

    private void Drop()
    {
        if (currentlyHeld != this) return;
        pickedUp = false;
        currentlyHeld = null;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }

        gameObject.layer = unheldLayer;

        if (dropSound != null && audioSource != null)
            audioSource.PlayOneShot(dropSound, dropSoundVolume);
    }

    private void UnwrapNoodle()
    {
        if (rawNoodlePrefab == null) return;

        GameObject noodle = Instantiate(rawNoodlePrefab, transform.position, transform.rotation);

        Pickup noodlePickup = noodle.GetComponent<Pickup>();
        if (noodlePickup != null)
        {
            noodlePickup.pickupRange = this.pickupRange;
            noodlePickup.smoothSpeed = this.smoothSpeed;
            noodlePickup.holdDistance = this.holdDistance;
            noodlePickup.heldRotation = this.heldRotation;

            StartCoroutine(DelayedPickup(noodlePickup));
        }

        if (unwrapSound != null && audioSource != null)
            audioSource.PlayOneShot(unwrapSound, unwrapSoundVolume);

        Destroy(gameObject, unwrapSound != null ? unwrapSound.length : 0.1f);
    }

    private IEnumerator DelayedPickup(Pickup pickup)
    {
        yield return new WaitForFixedUpdate();
        pickup.ForcePickup();
    }

    public void ForcePickup()
    {
        if (currentlyHeld != null) currentlyHeld.ForceDrop();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        pickedUp = true;
        currentlyHeld = this;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        gameObject.layer = heldLayer;

        if (pickupSound != null && audioSource != null)
            audioSource.PlayOneShot(pickupSound, pickupSoundVolume);
    }

    public void ForceDrop()
    {
        if (currentlyHeld == this)
        {
            pickedUp = false;
            currentlyHeld = null;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }

        gameObject.layer = unheldLayer;

        if (dropSound != null && audioSource != null)
            audioSource.PlayOneShot(dropSound, dropSoundVolume);
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