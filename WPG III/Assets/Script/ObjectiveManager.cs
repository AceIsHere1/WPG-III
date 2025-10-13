using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        objectiveText.text =
            "  Objective:\n" +
            "- Layani 2 pelanggan\n" +
            "- Buang sesajen sebelum hantu menangkap mu!!!\n" +
            "- Klik kiri untuk mengambil sesuatu";
    }
}
