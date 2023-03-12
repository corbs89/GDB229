using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPickup : MonoBehaviour
{
    [SerializeField] weaponStats weaponModel;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.WeaponPickup(weaponModel);

            Destroy(gameObject);
        }
    }
}
