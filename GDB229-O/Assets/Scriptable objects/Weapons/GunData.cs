using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Gun", menuName = "Weapons")]
public class GunData : ScriptableObject
{
    [SerializeField] public int damage;
    [SerializeField] public int fireRate;
    [SerializeField] public int magSize;
    [SerializeField] public int range;
    [SerializeField] public WeaponClass weaponClass;
}
