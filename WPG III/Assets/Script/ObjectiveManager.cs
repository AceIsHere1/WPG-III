using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        StartCoroutine(RunObjectives());
    }

    IEnumerator RunObjectives()
    {
        // Objective 1
        objectiveText.text = " Objective: Layani pelanggan pertama!";
        yield return new WaitForSeconds(10f); // tunggu 10 detik (bisa diubah)

        // Objective 2
        objectiveText.text = " Objective: Buang sesajen sebelum hantu muncul!";
        yield return new WaitForSeconds(10f);

        // Objective 3
        objectiveText.text = " Objective: Layani pelanggan berikutnya!";
        yield return new WaitForSeconds(10f);

        // Objective 4 (Final)
        objectiveText.text = " Semua objektif selesai! Klik kiri untuk mengambil sesuatu.";
    }
}
