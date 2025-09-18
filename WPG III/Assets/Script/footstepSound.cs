using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepSound : MonoBehaviour
{
    public AudioSource footstepsSound;

    void update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.enabled = false;
        }
    }
}