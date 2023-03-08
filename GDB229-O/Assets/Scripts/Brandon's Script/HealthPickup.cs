using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int refillHealthAmount = 10;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            hpPickup();
        }
        Destroy(gameObject);
    }
    void hpPickup()
    {
        GameManager.instance.playerController.increasehealth(refillHealthAmount);
    }
}

