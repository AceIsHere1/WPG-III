using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform playerCamera;
    public float holdDistance = 3f;
    public float smoothSpeed = 20f; 
    public float pickupRange = 2f; 
    public bool useDirectMovement = true; 

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
    private Quaternion initialRotationOffset;
    
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // Assign mixer group if set
        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;
    }

    void FixedUpdate()
    {
        if (pickedUp && currentlyHeld == this)
        {
            if (playerCamera != null)
            {
                Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
                
                if (rb != null)
                {
                    rb.isKinematic = true;
                    
                    if (useDirectMovement)
                    {
                        rb.MovePosition(targetPos);
                        
                        targetRotation = playerCamera.rotation * initialRotationOffset;
                        rb.MoveRotation(targetRotation);
                    }
                    else
                    {
                        rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.fixedDeltaTime * smoothSpeed));
                        
                        targetRotation = playerCamera.rotation * initialRotationOffset;
                        
                        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed));
                    }
                }
                else
                {
                    if (useDirectMovement)
                    {
                        transform.position = targetPos;
                        targetRotation = playerCamera.rotation * initialRotationOffset;
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * smoothSpeed);
                        
                        targetRotation = playerCamera.rotation * initialRotationOffset;
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed);
                    }
                }
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

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.transform == transform)
            {
                pickedUp = true;
                currentlyHeld = this;
                
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * transform.rotation;

                if (pickupSound != null && audioSource != null) 
                    audioSource.PlayOneShot(pickupSound, pickupSoundVolume);
            }
        }
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
        }
        
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
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (playerCamera != null)
            initialRotationOffset = Quaternion.Inverse(playerCamera.rotation) * transform.rotation;

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
        }
        
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