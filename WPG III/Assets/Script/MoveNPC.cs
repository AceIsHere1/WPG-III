using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveNPC : MonoBehaviour
{
    [SerializeField] Transform destination;

    NavMeshAgent navMeshAgent;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDestination()
    {
        if (destination != null)
        {
            Vector3 targetVector = destination.transform.position;

            navMeshAgent.SetDestination(targetVector);
        }
    }
}
