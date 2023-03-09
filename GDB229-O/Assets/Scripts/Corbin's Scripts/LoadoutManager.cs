using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Slot slot;

    public enum Slot
    {
        none,
        one,
        two
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gunPosition = GameManager.instance.playerController.GetGunPosition();
        GameManager.instance.playerController.ClearEquippedWeapon();
    }

    public void EquipWeaponInSlot1(GameObject newWeapon)
    {
        if (weaponSlot2 != null) ToggleSlot2(false);

        weaponSlot1Object = newWeapon;

        weaponSlot1 = Instantiate(weaponSlot1Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot1Renderer = weaponSlot1.GetComponent<Renderer>();

        weaponSlot1.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot1.transform.name = weaponSlot1.transform.name.Replace(stringToReplace, "");

        GameManager.instance.playerController.equippedWeapon = weaponSlot1;
        GameManager.instance.playerController.equippedWeapon.enabled = true;

        slot = Slot.one;
    }

    public void EquipWeaponInSlot2(GameObject newWeapon)
    {
        if (weaponSlot1 != null) ToggleSlot1(false);

        weaponSlot2Object = newWeapon;

        weaponSlot2 = Instantiate(weaponSlot2Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot2Renderer = weaponSlot2.GetComponent<Renderer>();

        weaponSlot2.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot2.transform.name = weaponSlot2.transform.name.Replace(stringToReplace, "");

        GameManager.instance.playerController.equippedWeapon = weaponSlot2;
        GameManager.instance.playerController.equippedWeapon.enabled = true;

        slot = Slot.two;
    }

    public void ToggleSlot1(bool value)
    {
        weaponSlot1.enabled = value;
        slot1Renderer.enabled = value;
    }

    public void ToggleSlot2(bool value)
    {
        weaponSlot2.enabled = value;
        slot2Renderer.enabled = value;
    }

    public void ClearSlot1()
    {
        ToggleSlot1(false);

        weaponSlot1Object = null;
        weaponSlot1 = null;
        slot1Renderer = null;
    }

    public void ClearSlot2()
    {
        ToggleSlot2(false);

        weaponSlot2Object = null;
        weaponSlot2 = null;
        slot2Renderer = null;
    }
}
