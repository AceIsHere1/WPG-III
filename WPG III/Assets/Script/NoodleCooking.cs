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

    // --- VARIABEL UNTUK MENGINGAT JENIS MIE ---
    private VarianMie mieYangSedangDimasak = VarianMie.BelumAdaIsi;

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

        float dist = Vector3.Distance(cam.position, transform.position);
        if (dist > interactDistance) return;

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        float sphereRadius = 0.5f;

        Debug.DrawRay(cam.position, cam.forward * interactDistance, Color.red, 2f);

        if (Physics.SphereCast(ray, sphereRadius, out hit, interactDistance))
        {
            NoodleCooking targetPot = hit.collider.GetComponentInParent<NoodleCooking>();

            if (targetPot != null && targetPot == this)
            {
                Pickup held = Pickup.GetCurrentlyHeld();

                if (isEmptyPot && !isCooking && !isCooked)
                {
                    // --- BAGIAN YANG DIUBAH: BACA KTP MIE MENTAH ---
                    if (held != null)
                    {
                        // Ambil komponen MieMentahData dari objek yang dipegang player
                        MieMentahData dataMentah = held.GetComponent<MieMentahData>();

                        // Pastikan objeknya punya script itu dan isinya bukan "BelumAdaIsi"
                        if (dataMentah != null && dataMentah.jenisMieMentah != VarianMie.BelumAdaIsi)
                        {
                            // 1. Panci mengingat jenis mie mentah yang dimasukkan
                            mieYangSedangDimasak = dataMentah.jenisMieMentah;

                            // 2. Hancurkan balok mie mentahnya (masuk ke panci)
                            held.ForceDrop();
                            Destroy(held.gameObject);

                            // 3. Mulai merebus
                            StartCoroutine(CookNoodles());
                        }
                        else
                        {
                            // Player pegang barang lain (bukan balok mie)
                            Debug.Log("Barang yang dipegang bukan mie mentah atau belum diset jenisnya!");
                        }
                    }
                }
                else if (isCooked && !isCooking)
                {
                    ServeToPlayer();
                }
            }
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

        // --- BAGIAN INI TETAP SAMA: SUNTIKKAN DATA KE MANGKOK ---
        MangkokData dataMangkok = bowl.GetComponent<MangkokData>();
        if (dataMangkok != null)
        {
            dataMangkok.isiMieSaatIni = mieYangSedangDimasak; // Pindahkan catatan panci ke mangkuk
        }
        else
        {
            Debug.LogWarning("Prefab Mangkuk tidak punya script MangkokData!");
        }

        Pickup bowlPickup = bowl.GetComponent<Pickup>();
        if (bowlPickup != null) bowlPickup.ForcePickup();

        // Reset panci kembali kosong
        SetVisualState(empty: true, boiling: false, cooked: false);

        isEmptyPot = true;
        isCooked = false;
        isCooking = false;
        mieYangSedangDimasak = VarianMie.BelumAdaIsi; // Reset catatan panci
    }

    private void SetVisualState(bool empty, bool boiling, bool cooked)
    {
        if (emptyPotVisual != null) emptyPotVisual.SetActive(empty);
        if (boilingPotVisual != null) boilingPotVisual.SetActive(boiling);
        if (cookedPotVisual != null) cookedPotVisual.SetActive(cooked);
    }
}