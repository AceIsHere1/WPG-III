using UnityEngine;

public class SimpleLookAt : MonoBehaviour
{
    public Transform target;  // Bu Inah
    public float rotateSpeed = 1.5f;

    private bool isRotating = false;
    private Quaternion targetRot;

    public void LookAtTarget()
    {
        targetRot = Quaternion.LookRotation(target.position - transform.position);
        isRotating = true;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

            // Cek kalau sudah hampir selesai rotasi → hentikan
            if (Quaternion.Angle(transform.rotation, targetRot) < 1f)
            {
                transform.rotation = targetRot;
                isRotating = false;
            }
        }
    }
}
