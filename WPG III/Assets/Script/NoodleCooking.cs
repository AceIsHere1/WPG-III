using UnityEngine;

public class NoodleCooking : MonoBehaviour
{
    public GameObject rawNoodlePrefab;       // mie kotak kuning
    public GameObject cookedNoodleBowlPrefab; // mangkok berisi mie matang
    public Transform bowlSpawnPoint;          // posisi spawn mangkok mie jadi

    private bool isCooking = false;
    private float cookTime = 5f; // lama masak 5 detik
    private float timer = 0f;

    void Update()
    {
        if (isCooking)
        {
            timer += Time.deltaTime;
            if (timer >= cookTime)
            {
                FinishCooking();
            }
        }
    }

    // Fungsi dipanggil saat player memasukkan mie mentah ke panci
    public void StartCooking()
    {
        if (!isCooking)
        {
            Debug.Log("Memasak mie...");
            isCooking = true;
            timer = 0f;
        }
    }

    private void FinishCooking()
    {
        isCooking = false;
        Debug.Log("Mie matang!");

        // Spawn mangkok mie matang
        Instantiate(cookedNoodleBowlPrefab, bowlSpawnPoint.position, bowlSpawnPoint.rotation);
    }
}
