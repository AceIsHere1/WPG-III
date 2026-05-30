using System.Collections;
using UnityEngine;

public class NoodleCookingScript2 : MonoBehaviour
{
    [Header("Visual Panci 2 Settings")]
    [Tooltip("Tarik EmptyPotVisual milik Panci 2 dari Hierarchy ke sini")]
    public GameObject emptyPotVisual;
    [Tooltip("Tarik BoilingPotVisual milik Panci 2 dari Hierarchy ke sini")]
    public GameObject boilingPotVisual;
    [Tooltip("Tarik CookedPotVisual milik Panci 2 dari Hierarchy ke sini")]
    public GameObject cookedPotVisual;

    [Header("Bowl Settings")]
    public GameObject bowlPrefab;
    public Transform spawnPoint;

    [Header("Cooking Settings")]
    public float cookingTime = 5f;
    public float interactDistance = 2f;

    public bool isEmptyPot = true;
    public bool isCooking = false;
    public bool isCooked = false;

    [Header("Sound Settings")]
    public AudioClip boilingSound;
    [Range(0f, 1f)] public float boilingSoundVolume = 1f;

    public AudioClip noodleReadySound;
    [Range(0f, 1f)] public float noodleReadySoundVolume = 1f;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;

        // Pastikan visual awal di-set dengan benar saat game mulai
        SetVisualState(empty: true, boiling: false, cooked: false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    private void TryInteract()
    {
        Transform cam = Camera.main != null ? Camera.main.transform : null;
        if (cam != null)
        {
            float dist = Vector3.Distance(cam.position, transform.position);
            if (dist > interactDistance) return;
        }

        // Asumsi class Pickup sudah ada di project kamu
        Pickup held = Pickup.GetCurrentlyHeld();

        if (isEmptyPot && !isCooking && !isCooked)
        {
            // Mulai merebus
            if (held != null && (held.CompareTag("RawNoodle") || held.gameObject.name.ToLower().Contains("mie kuning")))
            {
                held.ForceDrop();
                Destroy(held.gameObject);
                StartCoroutine(CookNoodles());
            }
        }
        else if (isCooked && !isCooking)
        {
            // Sajikan hasil rebusan
            ServeToPlayer();
        }
    }

    private IEnumerator CookNoodles()
    {
        if (isCooking) yield break;
        isCooking = true;
        isEmptyPot = false;
        isCooked = false;

        SetVisualState(empty: false, boiling: true, cooked: false);

        if (boilingSound != null)
            audioSource.PlayOneShot(boilingSound, boilingSoundVolume);

        yield return new WaitForSeconds(cookingTime);

        SetVisualState(empty: false, boiling: false, cooked: true);

        if (noodleReadySound != null)
            audioSource.PlayOneShot(noodleReadySound, noodleReadySoundVolume);

        isCooked = true;
        isCooking = false;
    }

    private void ServeToPlayer()
    {
        if (bowlPrefab == null) return;

        Transform cam = Camera.main != null ? Camera.main.transform : null;
        Vector3 spawnPos;
        Quaternion spawnRot = Quaternion.identity;

        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.position;
            spawnRot = spawnPoint.rotation;
        }
        else if (cam != null)
        {
            spawnPos = cam.position + cam.forward * 1f;
            spawnRot = cam.rotation;
        }
        else
        {
            spawnPos = transform.position + Vector3.up * 1f;
        }

        GameObject bowl = Instantiate(bowlPrefab, spawnPos, spawnRot);
        Pickup bowlPickup = bowl.GetComponent<Pickup>();
        if (bowlPickup != null) bowlPickup.ForcePickup();

        // Reset kembali ke panci kosong
        SetVisualState(empty: true, boiling: false, cooked: false);

        isEmptyPot = true;
        isCooked = false;
        isCooking = false;
    }

    private void SetVisualState(bool empty, bool boiling, bool cooked)
    {
        // Fungsi pengaman: memastikan objek visual tidak null sebelum di-aktif/non-aktifkan
        if (emptyPotVisual != null) emptyPotVisual.SetActive(empty);
        if (boilingPotVisual != null) boilingPotVisual.SetActive(boiling);
        if (cookedPotVisual != null) cookedPotVisual.SetActive(cooked);
    }
}