using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.WeaponPickup(weapon);

            Destroy(gameObject);
        }
    }
}
