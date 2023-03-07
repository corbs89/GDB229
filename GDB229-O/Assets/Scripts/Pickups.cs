using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    [SerializeField] GameObject model;
    [SerializeField] float speed = .2f;

    private void Update()
    {
        rotateobject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {



            Debug.Log("Item picked up");
        }

        Destroy(gameObject);
    }

    public void rotateobject()
    {
        transform.Rotate(0, speed, 0, Space.Self);
    }

    public void increasehealth(int amount)
    {

    }
    public void refillAmmo(int amount)
    {

    }
    public void increaseSpeed()
    {

    }

}

