using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    GameObject gunPosition;

    public GameObject weaponSlot1Object;
    public GameObject weaponSlot2Object;

    public Gun weaponSlot1;
    public Gun weaponSlot2;

    public Renderer slot1Renderer;
    public Renderer slot2Renderer;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        gunPosition = GameManager.instance.playerController.GetGunPosition();

        EquipWeaponInSlot2(weaponSlot2Object);
        EquipWeaponInSlot1(weaponSlot1Object);

        weaponSlot2.enabled = false;
        slot2Renderer.enabled = false;
    }

    public void EquipWeaponInSlot1(GameObject newWeapon)
    {
        weaponSlot1Object = newWeapon;

        weaponSlot1 = Instantiate(weaponSlot1Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot1Renderer = weaponSlot1.GetComponent<Renderer>();

        weaponSlot1.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot1.transform.name = weaponSlot1.transform.name.Replace(stringToReplace, "");

        GameManager.instance.playerController.equippedWeapon = weaponSlot1;

        GameManager.instance.playerController.equippedWeapon.enabled = true;
    }

    public void EquipWeaponInSlot2(GameObject newWeapon)
    {
        weaponSlot2Object = newWeapon;

        weaponSlot2 = Instantiate(weaponSlot2Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot2Renderer = weaponSlot2.GetComponent<Renderer>();

        weaponSlot2.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot2.transform.name = weaponSlot2.transform.name.Replace(stringToReplace, "");

        GameManager.instance.playerController.equippedWeapon = weaponSlot2;
        GameManager.instance.playerController.equippedWeapon.enabled = true;
    }

    public void ClearSlot1()
    {
        weaponSlot1.enabled = false;
        slot1Renderer.enabled = false;

        weaponSlot1Object = null;
        weaponSlot1 = null;
        slot1Renderer = null;
    }

    public void ClearSlot2()
    {
        weaponSlot2.enabled = false;
        slot2Renderer.enabled = false;

        weaponSlot2Object = null;
        weaponSlot2 = null;
        slot2Renderer = null;
    }
}
