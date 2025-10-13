using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    private int servedCustomers = 0;
    private int totalCustomers = 10;

    void Start()
    {
        UpdateObjectiveText();
    }

    void OnEnable()
    {
        // dengarkan event NPC dilayani
        GameEvents.OnNpcServed += OnNpcServed;
    }

    void OnDisable()
    {
        GameEvents.OnNpcServed -= OnNpcServed;
    }

    private void OnNpcServed()
    {
        servedCustomers++;
        UpdateObjectiveText();
    }

    void UpdateObjectiveText()
    {
        objectiveText.text = $"Pelanggan dilayani: {servedCustomers}/{totalCustomers}";
    }
}
