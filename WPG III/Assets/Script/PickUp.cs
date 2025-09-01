using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour

{
    public Transform pickupPoint;
    private Transform originalPoint;
    private bool positionSet;
    private bool pickedUp;
    void Start()
    {
        originalPoint = pickupPoint;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(pickedUp)
        {
            pickupPoint.transform.Translate(Camera.main.transform.forward * Time.deltaTime * 300 * Input.GetAxis("Mouse ScrollWheel"), Space.World);
        }
    }


    void OnMouseDown()
    {
        pickedUp = true;
        if(!positionSet)
        {
            positionSet = true;
            pickupPoint.position = transform.position;
        }
        transform.parent = pickupPoint.transform;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<BoxCollider>().enabled = false;
    }


    void OnMouseUp()
    {
        pickedUp = false;
        positionSet = false;
        pickupPoint.position = originalPoint.position;
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent <Rigidbody>().freezeRotation = false;
        GetComponent <BoxCollider>().enabled = true;
    }





}

