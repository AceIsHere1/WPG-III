using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveNPC : MonoBehaviour
{
    [SerializeField] Transform[] destinations;  // banyak tujuan

    NavMeshAgent navMeshAgent;
    int currentIndex = 0;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.Log("Nav Mesh Agent component not attached");
        }
        else
        {
            SetDestination();
        }
    }

    void Update()
    {
        if (destinations.Length == 0) return;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            if (currentIndex < destinations.Length - 1)
            {
                currentIndex++;
                SetDestination();
            }
            else
            {
                // berhenti di tujuan terakhir
                navMeshAgent.isStopped = true;
            }
        }
    }

    private void SetDestination()
    {
        if (destinations.Length == 0) return;

        Vector3 targetVector = destinations[currentIndex].position;
        navMeshAgent.SetDestination(targetVector);
    }
}
