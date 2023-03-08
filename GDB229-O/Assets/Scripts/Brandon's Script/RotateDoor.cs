using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    public float timer;
    public bool isOpening;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isOpening = true;
            transform.Rotate(0, 90, 0);
            timer += Time.deltaTime;
            if (timer > 10)
            {
                isOpening = false;
                transform.Rotate(0, -90, 0);
                timer = 0;
            }
        }
    }





}
