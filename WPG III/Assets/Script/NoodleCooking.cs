using System.Collections;
using UnityEngine;

public class NoodleCooking : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject cookedPotVisual;   // prefab panci matang
    public GameObject emptyPotVisual;    // prefab panci kosong
    public GameObject bowlPrefab;        // prefab mangkok jadi
    public Transform spawnPoint;         // optional

    [Header("Cooking Settings")]
    public float cookingTime = 5f;
    public bool isEmptyPot = true;

    [Header("Sound Settings")]
    public AudioClip noodleReadySound;
    private AudioSource audioSource;

    private bool isCooking = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TryInteract();
    }

    private void TryInteract()
    {
        Pickup held = Pickup.GetCurrentlyHeld();

        if (isEmptyPot)
        {
            if (held != null && (held.gameObject.CompareTag("RawNoodle") || held.gameObject.name.ToLower().Contains("mie kuning")))
            {
                held.ForceDrop();
                Destroy(held.gameObject);
                StartCoroutine(CookNoodles());
            }
        }
        else
        {
            ServeToPlayer();
        }
    }

    private IEnumerator CookNoodles()
    {
        if (isCooking) yield break;
        isCooking = true;
        yield return new WaitForSeconds(cookingTime);
        // Ganti visual panci jadi matang
        if (cookedPotVisual != null && emptyPotVisual != null)
        {
            emptyPotVisual.SetActive(false);
            cookedPotVisual.SetActive(true);
        }

        if (noodleReadySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(noodleReadySound);
        }

        isEmptyPot = false;
        isCooking = false;
        //if (cookedPotPrefab != null) Instantiate(cookedPotPrefab, transform.position, transform.rotation);
        //Destroy(gameObject);
    }

    private void ServeToPlayer()
    {
        if (bowlPrefab == null) return;
        Transform cam = Camera.main != null ? Camera.main.transform : null;
        Vector3 spawnPos;
        Quaternion spawnRot = Quaternion.identity;

        if (spawnPoint != null) { spawnPos = spawnPoint.position; spawnRot = spawnPoint.rotation; }
        else if (cam != null) { spawnPos = cam.position + cam.forward * 1f; spawnRot = cam.rotation; }
        else spawnPos = transform.position + Vector3.up * 1f;

        GameObject bowl = Instantiate(bowlPrefab, spawnPos, spawnRot);
        Pickup bowlPickup = bowl.GetComponent<Pickup>();
        if (bowlPickup != null) bowlPickup.ForcePickup();

        // Reset kembali ke panci kosong
        if (cookedPotVisual != null && emptyPotVisual != null)
        {
            cookedPotVisual.SetActive(false);
            emptyPotVisual.SetActive(true);
        }

        isEmptyPot = true;
        //Destroy(gameObject);
    }
}