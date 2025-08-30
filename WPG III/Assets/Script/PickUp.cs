using UnityEngine;

public class Pickup : MonoBehaviour

{
    public Transform pickupPoint;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void OnMouseDown()
    {
        transform.parent = pickupPoint.transform;
    }


    void OnMouseUp()
    {
        transform.parent = null;
    }





}

