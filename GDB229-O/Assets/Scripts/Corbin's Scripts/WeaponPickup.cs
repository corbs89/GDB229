using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject weapon;

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            switch (LoadoutManager.instance.slot)
            {
                case LoadoutManager.Slot.none:

                    LoadoutManager.instance.EquipWeaponInSlot1(weapon);
                    Destroy(gameObject);
                    break;

                case LoadoutManager.Slot.one:

                    if (LoadoutManager.instance.weaponSlot2Object == null)
                    {
                        LoadoutManager.instance.EquipWeaponInSlot2(weapon);
                        Destroy(gameObject);
                    }
                    break;

                case LoadoutManager.Slot.two:

                    if (LoadoutManager.instance.weaponSlot1Object == null)
                    {
                        LoadoutManager.instance.EquipWeaponInSlot1(weapon);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    public IEnumerator EnablePickup()
    {
        yield return new WaitForSeconds(3);

        GetComponent<SphereCollider>().enabled = true;
    }
}
