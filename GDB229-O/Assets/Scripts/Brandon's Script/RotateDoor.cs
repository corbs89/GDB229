using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    public float timer;
    public bool isOpen;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            openDoor();
        }
    }


    void openDoor()
    {
        transform.Rotate(0, 270, 0, Space.Self);
    }





}
