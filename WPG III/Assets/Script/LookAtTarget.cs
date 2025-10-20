using System.Collections;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [Tooltip("Camera transform that will rotate (e.g. Main Camera)")]
    public Transform playerCamera;

    [Tooltip("Target to look at (e.g. Bu Inah transform)")]
    public Transform target;

    [Tooltip("Speed multiplier for the slerp. Larger = faster")]
    public float rotateSpeed = 3f;

    [Tooltip("If true, only rotate horizontally (Y axis)")]
    public bool onlyRotateY = true;

    [Tooltip("Optional: Script that normally controls camera (e.g. mouse look). Will be disabled during rotation")]
    public MonoBehaviour cameraControlScript; // Drag your FirstPersonLook / PlayerController here

    private Coroutine rotateRoutine;

    public void RotateToTarget()
    {
        if (cameraControlScript != null)
            cameraControlScript.enabled = false; // Matikan kontrol kamera

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateSmooth());
    }

    IEnumerator RotateSmooth()
    {
        if (playerCamera == null || target == null)
            yield break;

        Quaternion finalRot = Quaternion.LookRotation(target.position - playerCamera.position);

        if (onlyRotateY)
        {
            Vector3 euler = finalRot.eulerAngles;
            euler.x = playerCamera.rotation.eulerAngles.x; // pertahankan pitch
            finalRot = Quaternion.Euler(euler);
        }

        while (Quaternion.Angle(playerCamera.rotation, finalRot) > 0.5f)
        {
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, finalRot, Time.deltaTime * rotateSpeed);
            yield return null;
        }

        playerCamera.rotation = finalRot;

        // Kalau mau aktifkan lagi kontrol kamera setelah rotasi:
        // if (cameraControlScript != null)
        //     cameraControlScript.enabled = true;
    }
}
