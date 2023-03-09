using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    ///[SerializeField] int amount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.CompareTag("Enemy"))
        {
            GameManager.instance.playerController.equippedWeapon.refillAmmo();
        }
        Destroy(gameObject);
    }






}
