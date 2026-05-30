using System.Collections;
using UnityEngine;

public class NoodleCooking : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject emptyPotVisual;     // prefab panci kosong
    public GameObject boilingPotVisual;   // prefab panci berisi mi saat direbus
    public GameObject cookedPotVisual;    // prefab panci berisi mi matang
    public GameObject bowlPrefab;         // prefab mangkok jadi
    public Transform spawnPoint;          // titik spawn mangkok opsional

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
        if (cam == null) return;

        // 1. Cek apakah pemain berada di dekat panci
        float dist = Vector3.Distance(cam.position, transform.position);
        if (dist > interactDistance) return;

        // 2. Buat garis Raycast lurus dari tengah kamera pemain
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        // (DEBUG) Gambar garis merah di Jendela Scene (selama 2 detik) untuk melihat arah tatapan
        Debug.DrawRay(cam.position, cam.forward * interactDistance, Color.red, 2f);

        // 3. Tembakkan garis tersebut sejauh jarak interaksi
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 4. Cari komponen NoodleCooking di objek yang ketabrak (atau di induk objek tersebut)
            NoodleCooking targetPot = hit.collider.GetComponentInParent<NoodleCooking>();

            // Jika script-nya ketemu dan benar-benar milik panci yang ini, lakukan aksi
            if (targetPot != null && targetPot == this)
            {
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
            else
            {
                // (DEBUG) Kasih tahu di console kalau pemain malah nabrak objek lain
                Debug.Log("Raycast kena objek: " + hit.collider.gameObject.name + ", bukan Panci ini.");
            }
        }
        else
        {
            // (DEBUG) Kasih tahu kalau pancinya nggak punya collider atau jarak terlalu jauh
            Debug.Log("Raycast tidak nabrak apa-apa! Pastikan Panci punya Box Collider.");
        }
    }

    private IEnumerator CookNoodles()
    {
        if (isCooking) yield break;
        isCooking = true;
        isEmptyPot = false;
        isCooked = false;

        SetVisualState(empty: false, boiling: true, cooked: false);

        // Mainkan suara rebusan dengan volume
        if (boilingSound != null)
            audioSource.PlayOneShot(boilingSound, boilingSoundVolume);

        yield return new WaitForSeconds(cookingTime);

        SetVisualState(empty: false, boiling: false, cooked: true);

        // Mainkan suara mi matang dengan volume
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
        if (emptyPotVisual != null) emptyPotVisual.SetActive(empty);
        if (boilingPotVisual != null) boilingPotVisual.SetActive(boiling);
        if (cookedPotVisual != null) cookedPotVisual.SetActive(cooked);
    }
}