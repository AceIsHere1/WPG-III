using UnityEngine;
using System.Collections;

public class NPCReceive : MonoBehaviour
{
    private bool hasMie = false;

    public void ReceiveMie()
    {
        if (!hasMie)
        {
            hasMie = true;
            Debug.Log($"{name} menerima mie!");

            // jalan pergi
            StartCoroutine(WalkAway());
        }
    }

    IEnumerator WalkAway()
    {
        Vector3 target = transform.position + (transform.forward * 5f);
        float speed = 2f;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
