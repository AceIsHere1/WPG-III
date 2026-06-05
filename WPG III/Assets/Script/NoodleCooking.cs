using System.Collections;
using UnityEngine;

public class NoodleCooking : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject emptyPotVisual;
    public GameObject boilingPotVisual;
    public GameObject cookedPotVisual;

    // --- DIUBAH: SEKARANG ADA 3 SLOT MANGKUK MATANG ---
    [Header("Bowl Prefabs (Mie Matang)")]
    public GameObject bowlPrefabUngu;
    public GameObject bowlPrefabKeriting;
    public GameObject bowlPrefabBiasa;

    public Transform spawnPoint;

    [Header("Cooking Settings")]
    public float cookingTime = 5f;
    public float interactDistance = 2f;

    public bool isEmptyPot = true;
    public bool isCooking = false;
    public bool isCooked = false;

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
                    if (held != null)
                    {
                        MieMentahData dataMentah = held.GetComponent<MieMentahData>();

                        if (dataMentah != null && dataMentah.jenisMieMentah != VarianMie.BelumAdaIsi)
                        {
                            mieYangSedangDimasak = dataMentah.jenisMieMentah;
                            held.ForceDrop();
                            Destroy(held.gameObject);
                            StartCoroutine(CookNoodles());
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

        // --- DIUBAH: PILIH PREFAB MANGKUK SESUAI JENIS MIE ---
        GameObject prefabYangAkanDiSpawn = null;

        switch (mieYangSedangDimasak)
        {
            case VarianMie.MieUnguSambalMatah:
                prefabYangAkanDiSpawn = bowlPrefabUngu;
                break;
            case VarianMie.MieGorengKeriting:
                prefabYangAkanDiSpawn = bowlPrefabKeriting;
                break;
            case VarianMie.MieGorengBiasa:
                prefabYangAkanDiSpawn = bowlPrefabBiasa;
                break;
        }

        // Jaga-jaga kalau lupa masukin prefab di Inspector
        if (prefabYangAkanDiSpawn == null)
        {
            Debug.LogError("Prefab mangkuk belum dimasukkan ke slot Inspector Panci!");
            return;
        }

        // Spawn mangkuk yang sudah dipilih
        GameObject bowl = Instantiate(prefabYangAkanDiSpawn, spawnPos, spawnRot);

        // --- SUNTIKKAN KTP KE MANGKOK BIAR NPC BISA BACA ---
        MangkokData dataMangkok = bowl.GetComponent<MangkokData>();
        if (dataMangkok != null)
        {
            dataMangkok.isiMieSaatIni = mieYangSedangDimasak;
        }
        else
        {
            Debug.LogWarning("Prefab Mangkuk tidak punya script MangkokData! NPC bakal nolak terus nih.");
        }

        Pickup bowlPickup = bowl.GetComponent<Pickup>();
        if (bowlPickup != null) bowlPickup.ForcePickup();

        SetVisualState(empty: true, boiling: false, cooked: false);

        isEmptyPot = true;
        isCooked = false;
        isCooking = false;
        mieYangSedangDimasak = VarianMie.BelumAdaIsi;
    }

    private void SetVisualState(bool empty, bool boiling, bool cooked)
    {
        if (emptyPotVisual != null) emptyPotVisual.SetActive(empty);
        if (boilingPotVisual != null) boilingPotVisual.SetActive(boiling);
        if (cookedPotVisual != null) cookedPotVisual.SetActive(cooked);
    }
}