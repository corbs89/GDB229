using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Gun
{
    [SerializeField] int amount = 25;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GrabAmmo();
        }
        Destroy(gameObject);
    }
    void GrabAmmo()
    {

        AddAmmo(amount);
        UpdateUI();

    }
}
